using System.Collections.Generic;
using Actors;
using Game.Phases.Movement.Systems;
using Game.Turns;
using Map.Views;

namespace Game.Phases.Movement
{
    public sealed class MovementPhase : ITurnPhase
    {
        private readonly MapView _mapView;
        private readonly IReadOnlyList<IMovementSystem> _systems;

        private MovementPhaseContext _context;
        private int _systemIndex;
        private bool _systemEntered;

        public MovementPhase(MapView mapView, IMovementSystem[] movementSystems)
        {
            _mapView = mapView;
            _systems = movementSystems;
        }

        public void Begin(TurnContext context)
        {
            // Cleanup phase because this is a new actor / turn context
            _mapView.ClearHighlightedTiles();
            _context = new MovementPhaseContext(context); // TODO avoid allocation through mutating
            _systemIndex = 0;
            _systemEntered = false;
        }

        public TurnPhaseStatus Tick()
        {
            // TODO debug block, remove
            if (_systemIndex >= _systems.Count)
                return TurnPhaseStatus.Running;

            // Much like TurnDirector, each phase moves through its systems for a given actor
            
            IMovementSystem system = _systems[_systemIndex];
            
            // Run a new system
            // TODO maybe rename Enter/Tick/Exit to align with ITurnPhase?
            if (!_systemEntered)
            {
                system.Enter(_context);
                _systemEntered = true;
            }

            // Keep running if not finished
            MovementSystemStatus status = system.Tick(_context);
            if (status == MovementSystemStatus.Running)
                return TurnPhaseStatus.Running;

            // system has finished, exit
            system.Exit(_context);
            _systemEntered = false;

            if (status == MovementSystemStatus.EndPhase)
                return TurnPhaseStatus.Complete;

            // increase the index, TurnPhaseStatus.Running will tell the TurnDirector
            // to tick again this phase, which will start with a new system
            _systemIndex++;
            return TurnPhaseStatus.Running;
        }

        public void End()
        {
            if (_context == null)
                return;

            if (_systemEntered && _systemIndex < _systems.Count)
                _systems[_systemIndex].Exit(_context);

            _mapView.ClearHighlightedTiles();
            _context = null;
            _systemEntered = false;
        }
    }
}
