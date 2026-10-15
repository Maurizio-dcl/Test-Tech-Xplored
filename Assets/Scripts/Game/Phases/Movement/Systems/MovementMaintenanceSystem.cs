using Actors;

namespace Game.Phases.Movement.Systems
{
    public sealed class MovementMaintenanceSystem : IMovementSystem
    {
        public void Enter(MovementPhaseContext context) { }

        public MovementSystemStatus Tick(MovementPhaseContext context)
        {
            // If this actor does not have the movement stat he cannot move,
            // so end the movement phase early.
            // I do this here instead of MovementPhase.Begin because it allows me to end the phase early automatically
            // without dedicated logic.
            if (!context.Turn.Actor.TryGetStat(StatDefinition.StatTag.Movement, out Stat movementStat))
                return MovementSystemStatus.EndPhase;

            movementStat.RestoreToMax();
            context.MovementStat = movementStat;
            return MovementSystemStatus.Complete;
        }

        public void Exit(MovementPhaseContext context) { }
    }
}
