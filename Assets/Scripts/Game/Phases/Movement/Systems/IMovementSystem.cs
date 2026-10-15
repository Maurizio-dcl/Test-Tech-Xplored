namespace Game.Phases.Movement.Systems
{
    // TODO could be abstracted to IPhaseSystem or something...
    public interface IMovementSystem
    {
        void Enter(MovementPhaseContext context);
        MovementSystemStatus Tick(MovementPhaseContext context);
        void Exit(MovementPhaseContext context);
    }
}