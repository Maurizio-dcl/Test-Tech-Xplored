using System;
using System.Collections.Generic;
using Actors;
using Actors.Actions;
using Actors.Actions.Definitions;
using Map;
using Utils;
using Action = System.Action;
using ActorAction = Actors.Actions.Action;

namespace Game.Input
{
    public sealed class BossActorInput : IActorInput
    {
        private static readonly List<CharacterType> OccupiedTileTypes =
            new((CharacterType[])Enum.GetValues(typeof(CharacterType)));

        private readonly Actor _boss;
        private readonly IReadOnlyList<Actor> _actors;
        private readonly Map.Map _map;

        // Reused between decisions to keep the explicit candidate lists allocation-free.
        private readonly List<Actor> _targetCandidates = new();
        private readonly List<ActorAction> _actionCandidates = new();
        private readonly List<Tile> _tileCandidates = new();
        private readonly Dictionary<Tile, int> _reachableTiles = new();

        private Actor _primaryTarget;
        private Actor _plannedTarget;
        private ActorAction _plannedAction;
        private Tile _plannedDestination;
        private InputPhase _phase;
        private bool _turnStarted;
        private bool _hasAggro;
        private bool _movementPlanned;
        private bool _moveSubmitted;
        private bool _moveSelectionExited;
        private bool _actionSubmitted;
        private bool _actionConfirmed;

        private enum InputPhase
        {
            None,
            Movement,
            Action
        }

        public BossActorInput(Actor boss, IReadOnlyList<Actor> actors, Map.Map map)
        {
            _boss = boss;
            _actors = actors;
            _map = map;
        }

        public event Action<Tile> TileSelected
        {
            add
            {
                BeginTurnIfNeeded();

                if (_phase == InputPhase.Action)
                {
                    SelectPlannedTarget(value);
                    return;
                }

                _phase = InputPhase.Movement;
                PlanMovement();

                if (_plannedDestination == null || _moveSubmitted)
                    return;

                _moveSubmitted = true;
                value?.Invoke(_plannedDestination);
            }
            remove
            {
                if (_phase == InputPhase.Movement && _moveSubmitted)
                    _moveSelectionExited = true;
            }
        }

        public event Action<ActorAction> ActionSelected
        {
            add
            {
                BeginTurnIfNeeded();
                _phase = InputPhase.Action;

                if (_actionConfirmed)
                    return;

                PlanActionAfterMovement();
                if (_plannedAction == null)
                    return;

                _actionSubmitted = true;
                value?.Invoke(_plannedAction);
            }
            remove { }
        }

        public event Action EndMovementRequested
        {
            add
            {
                BeginTurnIfNeeded();
                _phase = InputPhase.Movement;
                PlanMovement();

                if (_plannedDestination == null || _moveSelectionExited)
                    value?.Invoke();
            }
            remove { }
        }

        public event Action EndActionRequested
        {
            add
            {
                if (_actionConfirmed || _plannedAction == null)
                    value?.Invoke();
            }
            remove
            {
                if (_actionConfirmed || _plannedAction == null)
                    _turnStarted = false;
            }
        }

        public event Action CancelActionRequested
        {
            add { }
            remove { }
        }

        public event Action ConfirmTargetsRequested
        {
            add
            {
                if (!_actionSubmitted)
                    return;

                _actionConfirmed = true;
                value?.Invoke();
            }
            remove { }
        }

        private void BeginTurnIfNeeded()
        {
            // Setup phase for the boss' turn
            
            if (_turnStarted)
                return;

            _turnStarted = true;
            _phase = InputPhase.None;
            _primaryTarget = null;
            _plannedTarget = null;
            _plannedAction = null;
            _plannedDestination = null;
            _movementPlanned = false;
            _moveSubmitted = false;
            _moveSelectionExited = false;
            _actionSubmitted = false;
            _actionConfirmed = false;

            UpdateTargetCandidates();

            // If at least one hero has aggro, that's the target...
            _hasAggro = _targetCandidates.Count > 0;
            if (_hasAggro)
            {
                _primaryTarget = RandomElement(_targetCandidates);
                return;
            }

            // ...otherwise try to heal
            PlanHeal();
        }

        private void UpdateTargetCandidates()
        {
            int highestAggro = 0;
            _targetCandidates.Clear();

            for (int i = 0; i < _actors.Count; i++)
            {
                Actor hero = _actors[i];
                if (!IsAliveHero(hero))
                    continue;

                int aggro = GetAggro(hero);
                if (aggro <= 0)
                    continue;

                if (aggro > highestAggro)
                {
                    highestAggro = aggro;
                    _targetCandidates.Clear();
                }

                if (aggro == highestAggro)
                    _targetCandidates.Add(hero);
            }
        }

        private void PlanMovement()
        {
            if (_movementPlanned)
                return;

            _movementPlanned = true;
            if (_plannedAction != null || !_hasAggro)
                return;

            // Skip if we can act on the target without moving
            if (TrySelectRandomAction(_primaryTarget, out ActorAction action))
            {
                SetPlannedAction(action, _primaryTarget);
                return;
            }

            // Skip if we can't move or our target does not have a valid tile
            if (!_boss.TryGetStat(StatDefinition.StatTag.Movement, out Stat movement) ||
                movement.Value <= 0 ||
                !_map.TryGetActorTile(_primaryTarget, out Tile targetTile))
            {
                return;
            }

            _map.GetTilesInRange(
                _boss,
                1,
                movement.Value,
                _reachableTiles,
                _boss.Definition.BlockingType,
                OccupiedTileTypes);

            // Our movement target tile is/are the available tile(s) in closest distance to our target
            int closestDistance = int.MaxValue;
            _tileCandidates.Clear();
            foreach (Tile tile in _reachableTiles.Keys)
            {
                int distance = DistanceUtils.ManhattanDistance(tile.Position, targetTile.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    _tileCandidates.Clear();
                }

                if (distance == closestDistance)
                    _tileCandidates.Add(tile);
            }

            if (_tileCandidates.Count > 0)
                _plannedDestination = RandomElement(_tileCandidates);
        }

        private void PlanActionAfterMovement()
        {
            if (_plannedAction != null || !_hasAggro)
                return;

            if (TrySelectRandomAction(_primaryTarget, out ActorAction primaryAction))
            {
                SetPlannedAction(primaryAction, _primaryTarget);
                return;
            }

            // Since our primary target cannot be attacked, search for other heroes that can, 
            // prioritizing distance first and aggro second
            int closestDistance = int.MaxValue;
            int highestAggro = int.MinValue;
            _targetCandidates.Clear();

            for (int i = 0; i < _actors.Count; i++)
            {
                Actor candidate = _actors[i];
                
                // Skip the primary target, invalid heroes and heroes the boss cannot currently attack
                if (candidate == _primaryTarget || !IsAliveHero(candidate) ||
                    !HasAvailableActionFor(candidate, out int distance))
                    continue;

                int aggro = GetAggro(candidate);
                
                // A closer target, or one with more aggro at the same distance, 
                // replaces all previously found candidates
                if (distance < closestDistance ||
                    (distance == closestDistance && aggro > highestAggro))
                {
                    closestDistance = distance;
                    highestAggro = aggro;
                    _targetCandidates.Clear();
                }

                // Keep ties so it can be resolved randomly
                if (distance == closestDistance && aggro == highestAggro)
                    _targetCandidates.Add(candidate);
            }

            if (_targetCandidates.Count == 0)
                return;

            Actor fallbackTarget = RandomElement(_targetCandidates);
            if (TrySelectRandomAction(fallbackTarget, out ActorAction fallbackAction))
                SetPlannedAction(fallbackAction, fallbackTarget);
        }

        private void PlanHeal()
        {
            // Search for actions that have healing effects and can actually be used, select it if found
            
            _actionCandidates.Clear();
            for (int i = 0; i < _boss.Actions.Count; i++)
            {
                ActorAction action = _boss.Actions[i];
                if (!CanUseActionOn(action, _boss, 0) || !IsHealingAction(action))
                    continue;

                _actionCandidates.Add(action);
            }

            if (_actionCandidates.Count > 0)
                SetPlannedAction(RandomElement(_actionCandidates), _boss);
        }

        private bool TrySelectRandomAction(
            Actor target,
            out ActorAction selectedAction)
        {
            if (!TryGetDistanceTo(target, out int distance))
            {
                selectedAction = null;
                return false;
            }

            _actionCandidates.Clear();
            for (int i = 0; i < _boss.Actions.Count; i++)
            {
                ActorAction action = _boss.Actions[i];
                if (CanUseActionOn(action, target, distance))
                    _actionCandidates.Add(action);
            }

            if (_actionCandidates.Count == 0)
            {
                selectedAction = null;
                return false;
            }

            selectedAction = RandomElement(_actionCandidates);
            return true;
        }

        private bool HasAvailableActionFor(Actor target, out int distance)
        {
            if (!TryGetDistanceTo(target, out distance))
                return false;

            for (int i = 0; i < _boss.Actions.Count; i++)
            {
                if (CanUseActionOn(_boss.Actions[i], target, distance))
                    return true;
            }

            return false;
        }

        private static bool CanUseActionOn(ActorAction action, Actor target, int distance)
            => action.IsAvailable && action.Definition.CanTarget(target, distance);

        private bool TryGetDistanceTo(Actor target, out int distance)
        {
            if (target != null &&
                _map.TryGetActorTile(_boss, out Tile bossTile) &&
                _map.TryGetActorTile(target, out Tile targetTile))
            {
                distance = DistanceUtils.ManhattanDistance(bossTile.Position, targetTile.Position);
                return true;
            }

            distance = 0;
            return false;
        }

        private static bool IsHealingAction(ActorAction action)
        {
            IReadOnlyList<ActionEffectDefinition> effects = action.Definition.Effects;
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i] is HealEffectDefinition)
                    return true;
            }

            return false;
        }

        private void SelectPlannedTarget(Action<Tile> listener)
        {
            if (!_actionSubmitted || 
                _plannedTarget == null ||
                !_map.TryGetActorTile(_plannedTarget, out Tile targetTile))
            {
                return;
            }

            listener?.Invoke(targetTile);
        }

        private void SetPlannedAction(ActorAction action, Actor target)
        {
            _plannedAction = action;
            _plannedTarget = target;
        }
        
        private bool IsAliveHero(Actor actor) =>
            actor != _boss &&
            !actor.IsDead &&
            actor.Definition.CharacterType == CharacterType.Hero &&
            _map.TryGetActorTile(actor, out _);

        private static int GetAggro(Actor actor) =>
            actor.TryGetStat(StatDefinition.StatTag.Aggro, out Stat aggro) ? aggro.Value : 0;

        private static T RandomElement<T>(IReadOnlyList<T> candidates) =>
            candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }
}
