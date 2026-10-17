using System.Collections.Generic;
using Actors;
using Game.Turns;
using Map;

namespace Game.Phases.Movement
{
    public sealed class MovementPhaseContext
    {
        public TurnContext Turn { get; }
        public Stat MovementStat { get; set; }
        public IReadOnlyDictionary<Tile, int> ReachableTiles { get; set; }
        public Tile SelectedDestination { get; set; }

        public MovementPhaseContext(TurnContext turn)
        {
            Turn = turn;
            turn.Actor.TryGetStat(StatDefinition.StatTag.Movement, out Stat movementStat);
            MovementStat = movementStat;
        }
    }
}
