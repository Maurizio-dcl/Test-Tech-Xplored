namespace Game.Turns
{
    public interface ITurnPhase
    {
        void Begin(TurnContext context);
        TurnPhaseStatus Tick();
        void End();
    }
}