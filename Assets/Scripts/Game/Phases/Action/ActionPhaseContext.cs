using System.Collections.Generic;
using Actors;
using Game.Turns;
using Map;

namespace Game.Phases.Action
{
    public sealed class ActionPhaseContext
    {
        public TurnContext Turn { get; }
        public Actors.Actions.Action SelectedAction { get; set; }
        public IReadOnlyList<Actor> ValidTargets { get; set; }
        public IReadOnlyList<Actor> SelectedTargets { get; set; }

        public ActionPhaseContext(TurnContext turn)
        {
            Turn = turn;
        }
    }
}
