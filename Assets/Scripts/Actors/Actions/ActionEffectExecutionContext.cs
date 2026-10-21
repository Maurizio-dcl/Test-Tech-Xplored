using System.Collections.Generic;
using Game.Phases.Action;

namespace Actors.Actions
{
    public sealed class ActionEffectExecutionContext
    {
        private readonly List<IActionEffect> _outcomes;

        public ActionPhaseContext ActionContext { get; }
        public Actor Source => ActionContext.Turn.Actor;
        public Map.Map Map => ActionContext.Turn.Map;
        public IReadOnlyList<Actor> Targets => ActionContext.SelectedTargets;
        public IReadOnlyList<IActionEffect> Outcomes => _outcomes;

        public ActionEffectExecutionContext(
            ActionPhaseContext actionContext,
            List<IActionEffect> outcomes)
        {
            ActionContext = actionContext;
            _outcomes = outcomes;
        }

        public void AddOutcome(IActionEffect outcome) => _outcomes.Add(outcome);
    }
}
