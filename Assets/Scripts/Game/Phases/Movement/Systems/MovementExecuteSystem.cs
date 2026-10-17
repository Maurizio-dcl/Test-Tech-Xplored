using Map;

namespace Game.Phases.Movement.Systems
{
    public sealed class MovementExecuteSystem : IMovementSystem
    {
        public void Enter(MovementPhaseContext context)
        {
        }

        public MovementSystemStatus Tick(MovementPhaseContext context)
        {
            Tile destination = context.SelectedDestination;

            // Double check actor can actually move and can move on that tile
            if (!context.ReachableTiles.TryGetValue(destination, out int movementCost) ||
                movementCost > context.MovementStat.Value)
            {
                return MovementSystemStatus.Complete;
            }

            context.SelectedDestination = null;

            // First move the actor...
            if (!context.Turn.Map.Move(context.Turn.Actor, destination))
                return MovementSystemStatus.Complete;

            // ...only then subtract the cost
            context.MovementStat.Modify(-movementCost);

            // If the actor cannot move further end the phase straight away
            return context.MovementStat.Value > 0
                ? MovementSystemStatus.Complete
                : MovementSystemStatus.EndPhase;
        }

        public void Exit(MovementPhaseContext context)
        {
        }
    }
}