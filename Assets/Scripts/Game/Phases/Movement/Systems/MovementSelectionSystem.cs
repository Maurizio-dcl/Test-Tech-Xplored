using Game.Views;
using Map;
using Map.Views;

namespace Game.Phases.Movement.Systems
{
    public sealed class MovementSelectionSystem : IMovementSystem
    {
        private readonly MapView _mapView;
        private readonly MovementControlsView _controlsView;

        private MovementPhaseContext _context;
        private Tile _selectedTile;
        private bool _endMovementRequested;

        public MovementSelectionSystem(MapView mapView, MovementControlsView controlsView)
        {
            _mapView = mapView;
            _controlsView = controlsView;
        }

        public void Enter(MovementPhaseContext context)
        {
            _context = context;
            _selectedTile = null;
            _endMovementRequested = false;
            // Enable movement inputs only while this system is running
            _mapView.TileClicked += OnTileClicked;
            _controlsView.EndMovementRequested += OnEndMovementRequested;
            _controlsView.SetEndMovementAvailable(true);
        }

        public MovementSystemStatus Tick(MovementPhaseContext context)
        {
            if (_endMovementRequested)
                return MovementSystemStatus.EndPhase;

            if (_selectedTile == null)
                return MovementSystemStatus.Running;

            context.SelectedDestination = _selectedTile;
            return MovementSystemStatus.Complete;
        }

        public void Exit(MovementPhaseContext context)
        {
            _mapView.TileClicked -= OnTileClicked;
            _controlsView.EndMovementRequested -= OnEndMovementRequested;
            _controlsView.SetEndMovementAvailable(false);
            _mapView.ClearTilesColor();
            _context = null;
        }

        private void OnTileClicked(Tile tile)
        {
            // Check that the clicked tile is actually a valid tile the actor can move to
            if (_context.ReachableTiles.ContainsKey(tile))
                _selectedTile = tile;
        }

        private void OnEndMovementRequested() => _endMovementRequested = true;
    }
}
