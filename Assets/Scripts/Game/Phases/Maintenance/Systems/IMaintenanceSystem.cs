using Game.Turns;

namespace Game.Phases.Maintenance.Systems
{
    public interface IMaintenanceSystem
    {
        void Enter(TurnContext context);
        MaintenanceSystemStatus Tick(TurnContext context);
        void Exit(TurnContext context);
    }
}
