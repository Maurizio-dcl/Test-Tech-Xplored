using System;
using System.Collections.Generic;
using Actors;
using Actors.Actions;
using Map;

namespace Game.Phases.Action.Systems
{
    public sealed class ActionTargetCheckSystem : IActionSystem
    {
        public void Enter(ActionPhaseContext context)
        {
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            ActionDefinition definition = context.SelectedAction.Definition;
            Dictionary<Tile, int> tilesInRange = context.Turn.Map.GetTilesInRange(
                context.Turn.Actor,
                definition.MinRange,
                definition.MaxRange,
                blockingType: null);
            var validTargets = new List<Actor>();

            foreach (Tile tile in tilesInRange.Keys)
            {
                if (!tile.IsOccupied)
                    continue;

                Actor candidate = tile.Actor;
                CharacterType? typeFilter = definition.Targeting.TypeFilter;
                if (!typeFilter.HasValue || candidate.Definition.CharacterType == typeFilter.Value)
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
