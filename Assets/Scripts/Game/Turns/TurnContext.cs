using Actors;

namespace Game.Turns
{
    public readonly struct TurnContext
    {
        public int Round { get; }
        public Actor Actor { get; }
        public Map.Map Map { get; }

        public TurnContext(int round, Actor actor, Map.Map map)
        {
            Round = round;
            Actor = actor;
            Map = map;
        }
    }
}