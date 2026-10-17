using Actors;
using Game.Turns;

namespace Game.Phases.Maintenance.Systems
{
    public sealed class MovementMaintenanceSystem : IMaintenanceSystem
    {
        public void Enter(TurnContext context)
        {
        }

        public MaintenanceSystemStatus Tick(TurnContext context)
        {
            // If this actor does not have the movement stat he cannot move at all,
            // so end the movement phase early.
            // I do this here instead of MovementPhase.Begin because it allows me to end the phase early automatically
            // without dedicated logic.
            if (context.Actor.TryGetStat(StatDefinition.StatTag.Movement, out Stat movementStat))
                movementStat.RestoreToMax();

            return MaintenanceSystemStatus.Complete;
        }

        public void Exit(TurnContext context)
        {
        }
    }
}
