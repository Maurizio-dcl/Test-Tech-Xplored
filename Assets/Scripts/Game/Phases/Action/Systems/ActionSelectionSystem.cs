using Game.Input;
using Game.Views;

namespace Game.Phases.Action.Systems
{
    public sealed class ActionSelectionSystem : IActionSystem
    {
        private readonly ActorControlsView _controlsView;
        private readonly IActorInputResolver _inputResolver;

        private IActorInput _input;
        private Actors.Actions.Action _selectedAction;
        private bool _endActionRequested;

        public ActionSelectionSystem(ActorControlsView controlsView, IActorInputResolver inputResolver)
        {
            _controlsView = controlsView;
            _inputResolver = inputResolver;
        }
        
        public void Enter(ActionPhaseContext context)
        {
            _input = _inputResolver.Resolve(context.Turn.Actor);
            _selectedAction = null;
            _endActionRequested = false;
            _controlsView.ResetActionDescription();
            _controlsView.RefreshActions();
            _input.ActionSelected += OnActionSelected;
            _input.EndActionRequested += OnEndActionRequested;
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
            _input.ActionSelected -= OnActionSelected;
            _input.EndActionRequested -= OnEndActionRequested;
            _controlsView.SetEndActionAvailable(false);
            _input = null;
        }
        
        private void OnActionSelected(Actors.Actions.Action action)
        {
            if (action.IsAvailable)
                _selectedAction = action;
        }

        private void OnEndActionRequested() => _endActionRequested = true;
    }
}
