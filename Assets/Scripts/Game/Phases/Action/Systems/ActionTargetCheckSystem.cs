using System;
using System.Collections.Generic;
using Actors;
using Actors.Actions;
using Map;

namespace Game.Phases.Action.Systems
{
    public sealed class ActionTargetCheckSystem : IActionSystem
    {
        private readonly Dictionary<Tile, int> _tilesInRange = new();

        public void Enter(ActionPhaseContext context)
        {
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            ActionDefinition definition = context.SelectedAction.Definition;
            context.Turn.Map.GetTilesInRange(
                context.Turn.Actor,
                definition.MinRange,
                definition.MaxRange,
                _tilesInRange,
                blockingType: null);
            var validTargets = new List<Actor>();

            foreach (KeyValuePair<Tile, int> pair in _tilesInRange)
            {
                Tile tile = pair.Key;
                if (!tile.IsOccupied)
                    continue;

                Actor candidate = tile.Actor;
                if (definition.CanTarget(candidate, pair.Value))
                    validTargets.Add(candidate);
            }

            context.ValidTargets = validTargets;
            context.SelectedTargets = new List<Actor>();

            // Automatically select valid targets if no choice is possible
            if (definition.Targeting.Mode == TargetingMode.All || 
                (definition.Targeting.Mode == TargetingMode.Single && validTargets.Count <= 1))
                context.SelectedTargets = validTargets;

            return ActionSystemStatus.Complete;
        }

        public void Exit(ActionPhaseContext context)
        {
        }
    }
}
