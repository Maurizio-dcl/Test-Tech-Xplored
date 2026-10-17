namespace Game.Phases.Movement.Systems
{
    public interface IMovementSystem
    {
        void Enter(MovementPhaseContext context);
        MovementSystemStatus Tick(MovementPhaseContext context);
        void Exit(MovementPhaseContext context);
    }
}