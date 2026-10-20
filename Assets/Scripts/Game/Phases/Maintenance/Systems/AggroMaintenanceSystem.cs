using System.Collections.Generic;
using Actors;
using Game.Turns;

namespace Game.Phases.Maintenance.Systems
{
    public sealed class AggroMaintenanceSystem : IMaintenanceSystem
    {
        private const int AggroDecrementAmount = -1;

        private readonly IReadOnlyList<Actor> _actors;

        public AggroMaintenanceSystem(IReadOnlyList<Actor> actors)
        {
            _actors = actors;
        }

        public void Enter(TurnContext context)
        {
        }

        public MaintenanceSystemStatus Tick(TurnContext context)
        {
            // If this is the boss decrement anybody else
            if (context.Actor.Definition.CharacterType == CharacterType.Boss)
            {
                for (int i = 0; i < _actors.Count; i++)
                {
                    Actor actor = _actors[i];
                    if (actor != context.Actor)
                        DecrementAggro(actor);
                }
            }
            else
                DecrementAggro(context.Actor);

            return MaintenanceSystemStatus.Complete;
        }

        public void Exit(TurnContext context)
        {
        }

        private static void DecrementAggro(Actor actor)
        {
            if (actor.TryGetStat(StatDefinition.StatTag.Aggro, out Stat aggroStat))
                aggroStat.Modify(AggroDecrementAmount);
        }
    }
}