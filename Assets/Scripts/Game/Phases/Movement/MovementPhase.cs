using System.Collections.Generic;
using Actors;
using Game.Phases;
using Game.Phases.Movement.Systems;
using Game.Turns;
using Map.Views;

namespace Game.Phases.Movement
{
    public sealed class MovementPhase : ITurnPhase
    {
        private readonly MapView _mapView;
        private readonly IReadOnlyList<IMovementSystem> _setupSystems;
        private readonly IReadOnlyList<IMovementSystem> _repeatingSystems;

        private MovementPhaseContext _context;
        private IReadOnlyList<IMovementSystem> _systems;
        private int _systemIndex;
        private bool _systemEntered;
        private bool _runningSetupSystems;

        public MovementPhase(
            MapView mapView,
            IMovementSystem[] setupSystems,
            IMovementSystem[] repeatingSystems)
        {
            _mapView = mapView;
            _setupSystems = setupSystems;
            _repeatingSystems = repeatingSystems;
        }

        public void Begin(TurnContext context)
        {
            // Cleanup phase because this is a new actor / turn context
            _mapView.ClearTilesColor();
            _context = new MovementPhaseContext(context); // TODO avoid allocation through mutating
            _systems = _setupSystems;
            _systemIndex = 0;
            _systemEntered = false;
            _runningSetupSystems = true;
        }

        public TurnPhaseStatus Tick()
        {
            // Much like TurnDirector, each phase moves through its systems for a given actor
            
            // To allow a repeating cycle of systems I use this quick method,
            // not really that elegant, could be replaced by another nested state machine.
            // The phase first runs through the _setupSystems, then swaps them with the 
            // repeating ones.
            
            // If setup systems are finished swap them with the repeating ones
            if (_systemIndex >= _systems.Count)
            {
                _systems = _repeatingSystems;
                _systemIndex = 0;
                _runningSetupSystems = false;
                return TurnPhaseStatus.Running;
            }
            
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

            // Once the repeating systems are exhausted, the next tick will enter the first repeating system and so on
            if (!_runningSetupSystems && _systemIndex >= _systems.Count)
                _systemIndex = 0;

            return TurnPhaseStatus.Running;
        }

        public void End()
        {
            if (_context == null)
                return;

            if (_systemEntered && _systemIndex < _systems.Count)
                _systems[_systemIndex].Exit(_context);

            _mapView.ClearTilesColor();
            _context = null;
            _systemEntered = false;
        }
    }
}
