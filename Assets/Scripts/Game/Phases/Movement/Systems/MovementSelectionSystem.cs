using Game.Input;
using Game.Views;
using Map;
using Map.Views;

namespace Game.Phases.Movement.Systems
{
    public sealed class MovementSelectionSystem : IMovementSystem
    {
        private readonly MapView _mapView;
        private readonly ActorControlsView _controlsView;
        private readonly IActorInputResolver _inputResolver;

        private MovementPhaseContext _context;
        private IActorInput _input;
        private Tile _selectedTile;
        private bool _endMovementRequested;

        public MovementSelectionSystem(
            MapView mapView,
            ActorControlsView controlsView,
            IActorInputResolver inputResolver)
        {
            _mapView = mapView;
            _controlsView = controlsView;
            _inputResolver = inputResolver;
        }

        public void Enter(MovementPhaseContext context)
        {
            _context = context;
            _input = _inputResolver.Resolve(context.Turn.Actor);
            _selectedTile = null;
            _endMovementRequested = false;
            // Enable movement inputs only while this system is running
            _input.TileSelected += OnTileSelected;
            _input.EndMovementRequested += OnEndMovementRequested;
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
            _input.TileSelected -= OnTileSelected;
            _input.EndMovementRequested -= OnEndMovementRequested;
            _controlsView.SetEndMovementAvailable(false);
            _mapView.ClearTilesColor();
            _context = null;
            _input = null;
        }

        private void OnTileSelected(Tile tile)
        {
            // Check that the clicked tile is actually a valid tile the actor can move to
            if (_context.ReachableTiles.ContainsKey(tile))
                _selectedTile = tile;
        }

        private void OnEndMovementRequested() => _endMovementRequested = true;
    }
}
