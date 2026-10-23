using System.Collections.Generic;
using System.Linq;
using Actors;
using Game.Input;
using Game.Views;
using Map;
using Map.Views;
using UnityEngine;

namespace Game.Phases.Action.Systems
{
    public sealed class ActionTargetSelectionSystem : IActionSystem
    {
        private readonly MapView _mapView;
        private readonly ActorControlsView _controlsView;
        private readonly IActorInputResolver _inputResolver;
        private readonly Color _targetableTileColor;
        private readonly Dictionary<Tile, Actor> _targetsByTile = new();

        private IActorInput _input;
        private Actor _selectedTarget;
        private bool _selectionRequired;
        private bool _cancelActionRequested;

        public ActionTargetSelectionSystem(
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
            _selectedTarget = null;
            _cancelActionRequested = false;
            _targetsByTile.Clear();

            IReadOnlyList<Actor> validTargets = context.ValidTargets;
            
            // Selection is required only in this case, otherwise targets were auto-selected by the check system
            _selectionRequired = context.SelectedTargets.Count == 0 && context.ValidTargets.Count > 0;

            if (!_selectionRequired)
                return;

            _input = _inputResolver.Resolve(context.Turn.Actor);

            // The previous system already checked range and target type, get the tiles to highlight directly
            for (int i = 0; i < validTargets.Count; i++)
            {
                Actor target = validTargets[i];
                if (context.Turn.Map.TryGetActorTile(target, out Tile tile))
                    _targetsByTile.Add(tile, target);
            }

            _mapView.ClearTilesColor();
            _mapView.SetTilesColor(_targetsByTile.Keys.ToHashSet(), _targetableTileColor);
            _input.TileSelected += OnTileSelected;
            _input.CancelActionRequested += OnCancelActionRequested;
            _controlsView.SetCancelActionAvailable(true);
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            if (!_selectionRequired)
                return ActionSystemStatus.Complete;

            if (_cancelActionRequested)
                return ActionSystemStatus.CancelAction;

            if (_selectedTarget == null)
                return ActionSystemStatus.Running;

            context.SelectedTargets = new List<Actor> { _selectedTarget };
            return ActionSystemStatus.Complete;
        }

        public void Exit(ActionPhaseContext context)
        {
            if (_selectionRequired)
            {
                _input.TileSelected -= OnTileSelected;
                _input.CancelActionRequested -= OnCancelActionRequested;
                _controlsView.SetCancelActionAvailable(false);
            }

            _mapView.ClearTilesColor();
            _targetsByTile.Clear();
            _selectedTarget = null;
            _input = null;
            _selectionRequired = false;
            _cancelActionRequested = false;
        }

        private void OnTileSelected(Tile tile)
        {
            if (_targetsByTile.TryGetValue(tile, out Actor target))
                _selectedTarget = target;
        }

        private void OnCancelActionRequested() => _cancelActionRequested = true;
    }
}
