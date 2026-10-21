using System;
using UnityEngine;
using Utils;

namespace Actors.Actions.Definitions
{
    [Serializable]
    public sealed class NextDiceRollBonusEffectDefinition : ActionEffectDefinition
    {
        public NextDiceRollBonusEffectDefinition()
        {
        }

        public NextDiceRollBonusEffectDefinition(
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
            int bonus = Mathf.Max(0, ResolveValue(context));
            if (bonus == 0)
                return;

            for (int i = 0; i < context.Targets.Count; i++)
            {
                Actor target = context.Targets[i];
                target.AddNextDiceRollBonus(bonus);
                context.AddOutcome(new NextDiceRollBonusEffect(target, bonus));
            }
        }
    }
}
