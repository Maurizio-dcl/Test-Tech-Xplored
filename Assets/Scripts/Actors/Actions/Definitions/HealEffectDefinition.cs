using System;
using UnityEngine;
using Utils;

namespace Actors.Actions.Definitions
{
    [Serializable]
    public sealed class HealEffectDefinition : ActionEffectDefinition
    {
        public HealEffectDefinition()
        {
        }

        public HealEffectDefinition(
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
            int healing = Mathf.Max(0, ResolveValue(context));
            for (int i = 0; i < context.Targets.Count; i++)
            {
                Actor target = context.Targets[i];
                if (!target.TryGetStat(StatDefinition.StatTag.Hp, out Stat hp))
                    continue;

                int previousValue = hp.Value;
                hp.Modify(healing);
                context.AddOutcome(new HealEffect(target, hp.Value - previousValue));
            }
        }
    }
}