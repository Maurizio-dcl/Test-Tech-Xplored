using System;
using System.Collections.Generic;

namespace Actors.Actions.ValueProviders
{
    [Serializable]
    public sealed class HealedTargetCountValueProvider : EffectValueProvider
    {
        public override int GetValue(ActionEffectExecutionContext context)
        {
            var healedTargets = new HashSet<Actor>();
            for (int i = 0; i < context.Outcomes.Count; i++)
            {
                if (context.Outcomes[i] is HealEffect { Amount: > 0 } heal)
                    healedTargets.Add(heal.Target);
            }

            return healedTargets.Count;
        }
    }
}
