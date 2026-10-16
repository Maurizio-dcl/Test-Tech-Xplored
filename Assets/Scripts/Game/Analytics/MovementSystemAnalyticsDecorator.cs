using Game.Phases.Movement;
using Game.Phases.Movement.Systems;

namespace Game.Analytics
{
    // TODO change this for an action system decorator, movement is better logged by a dedicated log system
    //  bootstrapped in GameLoop that subscribes to move actor events from the map
    public class MovementSystemAnalyticsDecorator : IMovementSystem
    {
        private readonly IMovementSystem _system;
        
        public MovementSystemAnalyticsDecorator(IMovementSystem system)
        {
            _system = system;
        }

        public void Enter(MovementPhaseContext context)
        {
            throw new System.NotImplementedException();
        }

        public MovementSystemStatus Tick(MovementPhaseContext context)
        {
            throw new System.NotImplementedException();
        }

        public void Exit(MovementPhaseContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}