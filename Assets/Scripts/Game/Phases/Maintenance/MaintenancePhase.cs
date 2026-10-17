using System.Collections.Generic;
using Game.Phases.Maintenance.Systems;
using Game.Turns;

namespace Game.Phases.Maintenance
{
    public sealed class MaintenancePhase : ITurnPhase
    {
        private readonly IReadOnlyList<IMaintenanceSystem> _systems;

        private TurnContext _context;
        private int _systemIndex;
        private bool _systemEntered;

        public MaintenancePhase(IMaintenanceSystem[] systems)
        {
            _systems = systems;
        }

        public void Begin(TurnContext context)
        {
            _context = context;
            _systemIndex = 0;
            _systemEntered = false;
        }

        public TurnPhaseStatus Tick()
        {
            if (_systemIndex >= _systems.Count)
                return TurnPhaseStatus.Complete;

            IMaintenanceSystem system = _systems[_systemIndex];
            if (!_systemEntered)
            {
                system.Enter(_context);
                _systemEntered = true;
            }

            if (system.Tick(_context) == MaintenanceSystemStatus.Running)
                return TurnPhaseStatus.Running;

            system.Exit(_context);
            _systemEntered = false;
            _systemIndex++;

            return _systemIndex < _systems.Count
                ? TurnPhaseStatus.Running
                : TurnPhaseStatus.Complete;
        }

        public void End()
        {
            if (_systemEntered && _systemIndex < _systems.Count)
                _systems[_systemIndex].Exit(_context);

            _systemEntered = false;
        }
    }
}
