using System;
using UnityEngine;
using Utils;
using static Actors.StatDefinition;

namespace Actors.Actions.Definitions
{
    [Serializable]
    public sealed class DamageEffectDefinition : ActionEffectDefinition
    {
        public DamageEffectDefinition()
        {
        }

        public DamageEffectDefinition(
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
            int damage = Mathf.Max(0, ResolveValue(context));
            for (int i = 0; i < context.Targets.Count; i++)
            {
                Actor target = context.Targets[i];
                if (!target.TryGetStat(StatTag.Hp, out Stat hp))
                    continue;

                int previousValue = hp.Value;
                hp.Modify(-damage);
                context.AddOutcome(new DamageEffect(target, previousValue - hp.Value));
            }
        }
    }
}
