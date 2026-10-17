using Game.Views;

namespace Game.Phases.Action.Systems
{
    public sealed class ActionSelectionSystem : IActionSystem
    {
        private readonly ActorControlsView _controlsView;

        private Actors.Actions.Action _selectedAction;
        private bool _endActionRequested;

        public ActionSelectionSystem(ActorControlsView controlsView)
        {
            _controlsView = controlsView;
        }
        
        public void Enter(ActionPhaseContext context)
        {
            _selectedAction = null;
            _endActionRequested = false;
            _controlsView.RefreshActions();
            _controlsView.ActionRequested += OnActionRequested;
            _controlsView.EndActionPhaseRequested += OnEndActionRequested;
            _controlsView.SetEndActionAvailable(true);
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            if (context.Turn.Actor.Actions.Count == 0)
                return ActionSystemStatus.EndPhase;

            if (_endActionRequested)
                return ActionSystemStatus.EndPhase;

            if (_selectedAction == null)
                return ActionSystemStatus.Running;

            context.SelectedAction = _selectedAction;
            return ActionSystemStatus.Complete;
        }

        public void Exit(ActionPhaseContext context)
        {
            _controlsView.ActionRequested -= OnActionRequested;
            _controlsView.EndActionPhaseRequested -= OnEndActionRequested;
            _controlsView.SetEndActionAvailable(false);
        }
        
        private void OnActionRequested(Actors.Actions.Action action)
        {
            if (action.IsAvailable)
                _selectedAction = action;
        }

        private void OnEndActionRequested() => _endActionRequested = true;
    }
}
