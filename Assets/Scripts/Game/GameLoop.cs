using System.Collections.Generic;
using Actors;
using Game.Input;
using Game.Logging;
using Game.Phases;
using Game.Phases.Action;
using Game.Phases.Action.Systems;
using Game.Phases.Maintenance;
using Game.Phases.Maintenance.Systems;
using Game.Phases.Movement;
using Game.Phases.Movement.Systems;
using Game.Turns;
using Game.Views;
using Map.Views;
using UnityEngine;
using static Game.BossSpawnsDefinition;

namespace Game
{
    public sealed class GameLoop : MonoBehaviour
    {
        [SerializeField] private MapView mapView;
        [SerializeField] private ActorControlsView actorControlsView;
        [SerializeField] private EndGameView endGameView;
        [SerializeField] private BossSpawnsDefinition bossSpawnsDefinition;

        [SerializeField] private Color reachableTileColor = Color.green;
        [SerializeField] private Color targetableTileColor = Color.red;

        private Map.Map _map;
        private List<Actor> _actors;
        private TurnDirector _turnDirector;

        private void Start()
        {
            _map = new();
            mapView.Initialize(_map);

            _actors = InitializeActors();

            InitializeTurnDirector(_actors);
        }

        private void Update()
        {
            if (_turnDirector == null)
                return;

            _turnDirector.Tick();
            RemoveDeadActors();
        }

        private void OnDestroy() => _turnDirector?.Dispose();

        // Very simple check here, could handle this better but was running out of time
        private void RemoveDeadActors()
        {
            bool bossDied = false;
            bool heroDied = false;

            for (int i = _actors.Count - 1; i >= 0; i--)
            {
                Actor actor = _actors[i];
                if (!actor.IsDead)
                    continue;

                if (actor.Definition.CharacterType == CharacterType.Boss)
                    bossDied = true;
                else
                    heroDied = true;

                _turnDirector.RemoveActor(actor);
            }

            if (bossDied)
            {
                EndGame(true);
                return;
            }

            if (heroDied && !HasLivingHero())
                EndGame(false);
        }

        private bool HasLivingHero()
        {
            for (int i = 0; i < _actors.Count; i++)
            {
                if (_actors[i].Definition.CharacterType == CharacterType.Hero)
                    return true;
            }

            return false;
        }

        private void EndGame(bool victory)
        {
            _turnDirector.Dispose();
            _turnDirector = null;

            if (victory)
                endGameView.ShowVictory();
            else
                endGameView.ShowDefeat();
        }

        private List<Actor> InitializeActors()
        {
            int uniqueId = 0;
            List<ActorSpawn> heroSpawns = bossSpawnsDefinition.HeroSpawns;
            var actors = new List<Actor>(heroSpawns.Count + 1);

            for (int i = 0; i < heroSpawns.Count; i++)
                PlaceActor(uniqueId++, heroSpawns[i], actors);

            PlaceActor(uniqueId, bossSpawnsDefinition.BossSpawn, actors);

            return actors;
        }

        private void PlaceActor(int uniqueId, ActorSpawn spawn, List<Actor> actors)
        {
            if (spawn.definition == null)
            {
                Debug.LogError($"Initial actor placement {uniqueId} has no definition.", this);
                return;
            }

            if (spawn.definition.Visuals == null)
            {
                Debug.LogError($"Actor definition '{spawn.definition.name}' has no visuals.", this);
                return;
            }

            var actor = new Actor(uniqueId, spawn.definition);
            if (!_map.Move(actor, spawn.position))
            {
                Debug.LogError($"Could not place '{spawn.definition.name}' at {spawn.position}.", this);
                return;
            }

            actors.Add(actor);
        }

        private void InitializeTurnDirector(List<Actor> actors)
        {
            ILogSink logSink = new UnityConsoleLogSink();
            IActorInputResolver inputResolver = InitializeActorInputs(actors);

            // Initialize new phases here
            MaintenancePhase maintenancePhase = InitializeMaintenancePhase();
            MovementPhase movementPhase = InitializeMovementPhase(logSink, inputResolver);
            ActionPhase actionPhase = InitializeActionPhase(logSink, inputResolver);

            List<ITurnPhase> phases = new List<ITurnPhase> { maintenancePhase, movementPhase, actionPhase };

            _turnDirector = new TurnDirector(
                _map,
                actors,
                phases);
        }

        private IActorInputResolver InitializeActorInputs(IReadOnlyList<Actor> actors)
        {
            IActorInput mouseInput = new MouseActorInput(mapView, actorControlsView);
            var inputs = new Dictionary<Actor, IActorInput>(actors.Count);

            for (int i = 0; i < actors.Count; i++)
                inputs.Add(actors[i], mouseInput);

            return new ActorInputResolver(inputs);
        }

        private MaintenancePhase InitializeMaintenancePhase()
        {
            IMaintenanceSystem[] maintenanceSystems =
            {
                new MovementMaintenanceSystem(),
                new ActionMaintenanceSystem(actorControlsView)
            };

            return new(maintenanceSystems);
        }

        private MovementPhase InitializeMovementPhase(ILogSink logSink, IActorInputResolver inputResolver)
        {
            IMovementSystem[] movementSystems = { };

            IMovementSystem[] repeatingMovementSystems = new IMovementSystem[]
            {
                new MovementCheckSystem(mapView, reachableTileColor),
                new MovementSelectionSystem(mapView, actorControlsView, inputResolver),
                new MovementSystemLoggingDecorator(new MovementExecuteSystem(), logSink)
            };

            return new(mapView, movementSystems, repeatingMovementSystems);
        }

        private ActionPhase InitializeActionPhase(ILogSink logSink, IActorInputResolver inputResolver)
        {
            IActionSystem[] actionSystems = { };

            IActionSystem[] repeatingActionSystems = {
                new ActionSelectionSystem(actorControlsView, inputResolver),
                new ActionTargetCheckSystem(),
                new ActionTargetSelectionSystem(mapView, actorControlsView, inputResolver, targetableTileColor),
                new ActionTargetConfirmationSystem(mapView, actorControlsView, inputResolver, targetableTileColor),
                new ActionSystemLoggingDecorator(new ActionExecuteSystem(), logSink)
            };

            return new(mapView, actionSystems, repeatingActionSystems);
        }
    }
}
