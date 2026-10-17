namespace Game.Phases.Action.Systems
{
    public sealed class ActionExecuteSystem : IActionSystem
    {
        public void Enter(ActionPhaseContext context)
        {
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            Actors.Actions.Action action = context.SelectedAction;

            context.Outcome = action.Execute(context);
            action.SetOnCooldown();

            return ActionSystemStatus.Complete;
        }

        public void Exit(ActionPhaseContext context)
        {
        }
    }
}
