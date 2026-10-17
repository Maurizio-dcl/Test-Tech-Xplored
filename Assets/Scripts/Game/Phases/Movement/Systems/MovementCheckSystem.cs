using System;
using System.Collections.Generic;
using System.Linq;
using Actors;
using Map.Views;
using UnityEngine;

namespace Game.Phases.Movement.Systems
{
    public sealed class MovementCheckSystem : IMovementSystem
    {
        private static readonly List<CharacterType> OccupiedTileTypes =
            new((CharacterType[])Enum.GetValues(typeof(CharacterType)));

        private readonly MapView _mapView;
        private readonly Color _reachableTileColor;

        public MovementCheckSystem(MapView mapView, Color reachableTileColor)
        {
            _mapView = mapView;
            _reachableTileColor = reachableTileColor;
        }

        public void Enter(MovementPhaseContext context)
        {
        }

        public MovementSystemStatus Tick(MovementPhaseContext context)
        {
            // Remove the result of the previous movement action before calculating a new one.
            _mapView.ClearTilesColor();

            // Actors without a movement stat cannot take part in this phase.
            if (context.MovementStat == null || context.MovementStat.Value <= 0)
                return MovementSystemStatus.EndPhase;

            var reachableTiles = context.Turn.Map.GetTilesInRange(
                context.Turn.Actor,
                1,
                context.MovementStat.Value,
                context.Turn.Actor.Definition.BlockingType,
                OccupiedTileTypes);

            context.ReachableTiles = reachableTiles;

            if (reachableTiles.Count == 0)
                return MovementSystemStatus.EndPhase;

            _mapView.SetTilesColor(reachableTiles.Keys.ToHashSet(), _reachableTileColor);

            return MovementSystemStatus.Complete;
        }

        public void Exit(MovementPhaseContext context)
        {
        }
    }
}
