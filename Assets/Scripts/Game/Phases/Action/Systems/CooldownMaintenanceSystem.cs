using System.Collections.Generic;

namespace Game.Phases.Action.Systems
{
    public sealed class CooldownMaintenanceSystem : IActionSystem
    {
        private const int CooldownDecrementAmount = -1;
        
        public void Enter(ActionPhaseContext context)
        {
        }

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            IReadOnlyList<Actors.Actions.Action> actions = context.Turn.Actor.Actions;
            
            if (actions.Count == 0)
                return ActionSystemStatus.EndPhase;

            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].ModifyCooldown(CooldownDecrementAmount);
            }
            
            return ActionSystemStatus.Complete;
        }

        public void Exit(ActionPhaseContext context)
        {
        }
    }
}