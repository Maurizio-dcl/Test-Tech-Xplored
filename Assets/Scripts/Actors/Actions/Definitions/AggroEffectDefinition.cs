using System;
using Utils;

namespace Actors.Actions.Definitions
{
    [Serializable]
    public sealed class AggroEffectDefinition : ActionEffectDefinition
    {
        public AggroEffectDefinition()
        {
        }

        public AggroEffectDefinition(
            int baseValue,
            bool hasBonusDice = false,
            DiceExpression bonusDice = default,
            float multiplier = 1f,
            EffectValueProvider contextualValue = null)
            : base(baseValue, hasBonusDice, bonusDice, multiplier, contextualValue)
        {
        }

        public override void Execute(ActionEffectExecutionContext context)
        {
            if (!context.Source.TryGetStat(StatDefinition.StatTag.Aggro, out Stat aggro))
                return;

            int previousValue = aggro.Value;
            aggro.Modify(ResolveValue(context));
            context.AddOutcome(new AggroEffect(context.Source, aggro.Value - previousValue));
        }
    }
}