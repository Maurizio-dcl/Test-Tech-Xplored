using Game.Turns;

namespace Game.Phases
{
    public interface ITurnPhase
    {
        void Begin(TurnContext context);
        TurnPhaseStatus Tick();
        void End();
    }
}