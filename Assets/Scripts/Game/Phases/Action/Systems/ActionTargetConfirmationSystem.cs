using System.Collections.Generic;
using Actors;
using Game.Input;
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
        private readonly IActorInputResolver _inputResolver;
        private readonly Color _targetableTileColor;

        private IActorInput _input;
        private bool _confirmTargetsRequested;
        private bool _cancelActionRequested;

        public ActionTargetConfirmationSystem(
            MapView mapView,
            ActorControlsView controlsView,
            IActorInputResolver inputResolver,
            Color targetableTileColor)
        {
            _mapView = mapView;
            _controlsView = controlsView;
            _inputResolver = inputResolver;
            _targetableTileColor = targetableTileColor;
        }

        public void Enter(ActionPhaseContext context)
        {
            _input = _inputResolver.Resolve(context.Turn.Actor);
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

            _input.ConfirmTargetsRequested += OnConfirmTargetsRequested;
            _input.CancelActionRequested += OnCancelActionRequested;
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
            _input.ConfirmTargetsRequested -= OnConfirmTargetsRequested;
            _input.CancelActionRequested -= OnCancelActionRequested;
            _controlsView.SetConfirmTargetsAvailable(false);
            _controlsView.SetCancelActionAvailable(false);
            _input = null;
        }

        private void OnConfirmTargetsRequested() => _confirmTargetsRequested = true;

        private void OnCancelActionRequested() => _cancelActionRequested = true;
    }
}
