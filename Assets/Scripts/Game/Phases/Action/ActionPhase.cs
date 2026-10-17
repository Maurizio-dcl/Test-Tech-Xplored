using System.Collections.Generic;
using Game.Phases.Action.Systems;
using Game.Turns;
using Map.Views;

namespace Game.Phases.Action
{
    public sealed class ActionPhase : ITurnPhase
    {
        private readonly MapView _mapView;
        private readonly IReadOnlyList<IActionSystem> _setupSystems;
        private readonly IReadOnlyList<IActionSystem> _repeatingSystems;
        
        private ActionPhaseContext _context;
        private IReadOnlyList<IActionSystem> _systems;
        private int _systemIndex;
        private bool _systemEntered;
        private bool _runningSetupSystems;

        public ActionPhase(
            MapView mapView,
            IActionSystem[] setupSystems,
            IActionSystem[] repeatingSystems)
        {
            _mapView = mapView;
            _setupSystems = setupSystems;
            _repeatingSystems = repeatingSystems;
        }
        
        public void Begin(TurnContext context)
        {
            _mapView.ClearTilesColor();
            _context = new ActionPhaseContext(context); // TODO avoid allocation through mutating
            _systems = _setupSystems;
            _systemIndex = 0;
            _systemEntered = false;
            _runningSetupSystems = true;
        }

        public TurnPhaseStatus Tick()
        {
            if (_systemIndex >= _systems.Count)
            {
                _systems = _repeatingSystems;
                _systemIndex = 0;
                _runningSetupSystems = false;
                return TurnPhaseStatus.Running;
            }
            
            IActionSystem system = _systems[_systemIndex];
            
            // TODO maybe rename Enter/Tick/Exit to align with ITurnPhase?
            if (!_systemEntered)
            {
                system.Enter(_context);
                _systemEntered = true;
            }

            ActionSystemStatus status = system.Tick(_context);
            if (status == ActionSystemStatus.Running)
                return TurnPhaseStatus.Running;

            system.Exit(_context);
            _systemEntered = false;

            if (status == ActionSystemStatus.EndPhase)
                return TurnPhaseStatus.Complete;

            // Not elegant, as mentioned in the MovementPhase class this could be better handled with another
            // nested state machine
            if (status == ActionSystemStatus.CancelAction)
            {
                _systems = _repeatingSystems;
                _systemIndex = 0;
                _runningSetupSystems = false;
                return TurnPhaseStatus.Running;
            }

            _systemIndex++;

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
