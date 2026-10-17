using Game.Views;

namespace Game.Phases.Action.Systems
{
    public sealed class ActionSelectionSystem : IActionSystem
    {
        private readonly ActionControlsView _controlsView;

        private ActionPhaseContext _context;
        private Actors.Actions.Action _selectedAction;
        private bool _endActionRequested;

        public ActionSelectionSystem(ActionControlsView controlsView)
        {
            _controlsView = controlsView;
        }
        
        public void Enter(ActionPhaseContext context)
        {
            _context = context;
            _selectedAction = null;
            _endActionRequested = false;
            // _controlsView.ActionClicked += OnActionClicked;
            // _controlsView.EndActionRequested += OnEndActionRequested;
            // _controlsView.SetEndActionAvailable(true);
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            if (_endActionRequested)
                return ActionSystemStatus.EndPhase;

            if (_selectedAction == null)
                return ActionSystemStatus.Running;

            context.SelectedAction = _selectedAction;
            return ActionSystemStatus.Complete;
        }

        public void Exit(ActionPhaseContext context)
        {
            // _controlsView.ActionClicked -= OnActionClicked;
            // _controlsView.EndActionRequested -= OnEndActionRequested;
            // _controlsView.SetEndActionAvailable(false);
            _context = null;
        }
        
        private void OnActionClicked(Actors.Actions.Action action)
        {
            if (action.IsAvailable)
                _selectedAction = action;
        }

        private void OnEndActionRequested() => _endActionRequested = true;
    }
}