using System;
using System.Collections.Generic;
using System.Linq;
using Actors;
using Actors.Actions;
using Map;
using Map.Views;
using UnityEngine;

namespace Game.Phases.Action.Systems
{
    public sealed class ActionTargetSelectionSystem : IActionSystem
    {
        private readonly MapView _mapView;
        private readonly Color _targetableTileColor;
        private readonly Dictionary<Tile, Actor> _targetsByTile = new();

        private Actor _selectedTarget;
        private bool _selectionRequired;

        public ActionTargetSelectionSystem(MapView mapView, Color targetableTileColor)
        {
            _mapView = mapView;
            _targetableTileColor = targetableTileColor;
        }

        public void Enter(ActionPhaseContext context)
        {
            _selectedTarget = null;
            _targetsByTile.Clear();

            IReadOnlyList<Actor> validTargets = context.ValidTargets;
            
            // Selection is required only in this case, otherwise targets were auto-selected by the check system
            _selectionRequired = context.SelectedTargets.Count == 0 && context.ValidTargets.Count > 0;

            if (!_selectionRequired)
                return;

            // Get targets tiles to highlight (nothing blocks the flood fill and all occupied tiles are allowed
            ActionDefinition definition = context.SelectedAction.Definition;
            Dictionary<Tile, int> tilesInRange = context.Turn.Map.GetTilesInRange(
                context.Turn.Actor,
                definition.MinRange,
                definition.MaxRange,
                blockingType: null);
            
            // Filter only valid targets by type
            var validTargetSet = new HashSet<Actor>(validTargets);
            foreach (Tile tile in tilesInRange.Keys)
            {
                if (tile.IsOccupied && validTargetSet.Contains(tile.Actor))
                    _targetsByTile.Add(tile, tile.Actor);
            }

            _mapView.ClearTilesColor();
            _mapView.SetTilesColor(_targetsByTile.Keys.ToHashSet(), _targetableTileColor);
            _mapView.TileClicked += OnTileClicked;
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            if (!_selectionRequired)
                return ActionSystemStatus.Complete;

            if (_selectedTarget == null)
                return ActionSystemStatus.Running;

            context.SelectedTargets = new List<Actor> { _selectedTarget };
            return ActionSystemStatus.Complete;
        }

        public void Exit(ActionPhaseContext context)
        {
            if (_selectionRequired)
                _mapView.TileClicked -= OnTileClicked;

            _mapView.ClearTilesColor();
            _targetsByTile.Clear();
            _selectedTarget = null;
            _selectionRequired = false;
        }

        private void OnTileClicked(Tile tile)
        {
            if (_targetsByTile.TryGetValue(tile, out Actor target))
                _selectedTarget = target;
        }
    }
}
