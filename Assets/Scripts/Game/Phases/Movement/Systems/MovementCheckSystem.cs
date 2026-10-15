using Map.Views;
using UnityEngine;

namespace Game.Phases.Movement.Systems
{
    public sealed class MovementCheckSystem : IMovementSystem
    {
        private readonly MapView _mapView;
        private readonly Color _reachableTileColor;

        public MovementCheckSystem(MapView mapView, Color reachableTileColor)
        {
            _mapView = mapView;
            _reachableTileColor = reachableTileColor;
        }

        public void Enter(MovementPhaseContext context) { }

        public MovementSystemStatus Tick(MovementPhaseContext context)
        {
            var reachableTiles = context.Turn.Map.GetReachableTiles(
                context.Turn.Actor,
                context.MovementStat.Value,
                context.Turn.Actor.Definition.BlockingType);

            context.ReachableTiles = reachableTiles;
            _mapView.SetTilesColor(reachableTiles.Keys, _reachableTileColor);

            return MovementSystemStatus.Complete;
        }

        public void Exit(MovementPhaseContext context) { }
    }
}
