using System.Collections.Generic;
using Game.Turns;
using Game.Views;

namespace Game.Phases.Maintenance.Systems
{
    public sealed class ActionMaintenanceSystem : IMaintenanceSystem
    {
        private const int CooldownDecrementAmount = -1;

        private readonly ActorControlsView _controlsView;

        public ActionMaintenanceSystem(ActorControlsView controlsView)
        {
            _controlsView = controlsView;
        }

        public void Enter(TurnContext context)
        {
        }

        public MaintenanceSystemStatus Tick(TurnContext context)
        {
            IReadOnlyList<Actors.Actions.Action> actions = context.Actor.Actions;
            for (int i = 0; i < actions.Count; i++)
                actions[i].ModifyCooldown(CooldownDecrementAmount);

            _controlsView.SetupStats(context.Actor);
            _controlsView.SetupActions(context.Actor);

            return MaintenanceSystemStatus.Complete;
        }

        public void Exit(TurnContext context)
        {
        }
    }
}
