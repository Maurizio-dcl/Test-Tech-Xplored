using System;
using Game.Views;
using Map;
using Map.Views;
using ActorAction = Actors.Actions.Action;

namespace Game.Input
{
    public sealed class MouseActorInput : IActorInput
    {
        private readonly MapView _mapView;
        private readonly ActorControlsView _controlsView;

        public MouseActorInput(MapView mapView, ActorControlsView controlsView)
        {
            _mapView = mapView;
            _controlsView = controlsView;
        }

        public event Action<Tile> TileSelected
        {
            add => _mapView.TileClicked += value;
            remove => _mapView.TileClicked -= value;
        }

        public event Action<ActorAction> ActionSelected
        {
            add => _controlsView.ActionRequested += value;
            remove => _controlsView.ActionRequested -= value;
        }

        public event Action EndMovementRequested
        {
            add => _controlsView.EndMovementPhaseRequested += value;
            remove => _controlsView.EndMovementPhaseRequested -= value;
        }

        public event Action EndActionRequested
        {
            add => _controlsView.EndActionPhaseRequested += value;
            remove => _controlsView.EndActionPhaseRequested -= value;
        }

        public event Action CancelActionRequested
        {
            add => _controlsView.CancelActionRequested += value;
            remove => _controlsView.CancelActionRequested -= value;
        }

        public event Action ConfirmTargetsRequested
        {
            add => _controlsView.ConfirmTargetsRequested += value;
            remove => _controlsView.ConfirmTargetsRequested -= value;
        }
    }
}
