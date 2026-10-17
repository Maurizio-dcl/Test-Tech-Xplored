namespace Game.Phases.Action.Systems
{
    public interface IActionSystem
    {
        void Enter(ActionPhaseContext context);
        ActionSystemStatus Tick(ActionPhaseContext context);
        void Exit(ActionPhaseContext context);
    }
}