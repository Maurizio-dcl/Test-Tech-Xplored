using System.Collections.Generic;
using Actors;
using Game.Views;
using Map;
using Map.Views;
using UnityEngine;

namespace Game.Phases.Action.Systems
{
    public sealed class ActionTargetConfirmationSystem : IActionSystem
    {
        private readonly MapView _mapView;
        private readonly ActorControlsView _controlsView;
        private readonly Color _targetableTileColor;

        private bool _confirmTargetsRequested;
        private bool _cancelActionRequested;

        public ActionTargetConfirmationSystem(
            MapView mapView,
            ActorControlsView controlsView,
            Color targetableTileColor)
        {
            _mapView = mapView;
            _controlsView = controlsView;
            _targetableTileColor = targetableTileColor;
        }

        public void Enter(ActionPhaseContext context)
        {
            _confirmTargetsRequested = false;
            _cancelActionRequested = false;

            var selectedTargetTiles = new HashSet<Tile>();
            IReadOnlyList<Actor> selectedTargets = context.SelectedTargets;
            for (int i = 0; i < selectedTargets.Count; i++)
            {
                if (context.Turn.Map.TryGetActorTile(selectedTargets[i], out Tile tile))
                    selectedTargetTiles.Add(tile);
            }

            _mapView.ClearTilesColor();
            _mapView.SetTilesColor(selectedTargetTiles, _targetableTileColor);

            _controlsView.ConfirmTargetsRequested += OnConfirmTargetsRequested;
            _controlsView.CancelActionRequested += OnCancelActionRequested;
            _controlsView.SetConfirmTargetsAvailable(true);
            _controlsView.SetCancelActionAvailable(true);
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            if (_cancelActionRequested)
                return ActionSystemStatus.CancelAction;

            return _confirmTargetsRequested
                ? ActionSystemStatus.Complete
                : ActionSystemStatus.Running;
        }

        public void Exit(ActionPhaseContext context)
        {
            _mapView.ClearTilesColor();
            _controlsView.ConfirmTargetsRequested -= OnConfirmTargetsRequested;
            _controlsView.CancelActionRequested -= OnCancelActionRequested;
            _controlsView.SetConfirmTargetsAvailable(false);
            _controlsView.SetCancelActionAvailable(false);
        }

        private void OnConfirmTargetsRequested() => _confirmTargetsRequested = true;

        private void OnCancelActionRequested() => _cancelActionRequested = true;
    }
}
