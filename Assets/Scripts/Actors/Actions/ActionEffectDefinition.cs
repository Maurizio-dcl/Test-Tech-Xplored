using System;
using UnityEngine;
using Utils;

namespace Actors.Actions
{
    [Serializable]
    public abstract class ActionEffectDefinition
    {
        [SerializeField] private int baseValue = 1;
        [SerializeField] private bool hasBonusDice;
        [SerializeField] private DiceExpression bonusDice;
        [SerializeField] private float multiplier = 1f;
        [SerializeReference] private EffectValueProvider contextualValue;

        protected ActionEffectDefinition()
        {
        }

        protected ActionEffectDefinition(
            int baseValue,
            bool hasBonusDice,
            DiceExpression bonusDice,
            float multiplier,
            EffectValueProvider contextualValue = null)
        {
            this.baseValue = baseValue;
            this.hasBonusDice = hasBonusDice;
            this.bonusDice = bonusDice;
            this.multiplier = multiplier;
            this.contextualValue = contextualValue;
        }

        public abstract void Execute(ActionEffectExecutionContext context);

        protected int ResolveValue(ActionEffectExecutionContext context)
        {
            int diceValue = hasBonusDice ? DiceRoller.Roll(bonusDice).Total : 0;
            int contextValue = contextualValue?.GetValue(context) ?? 0;
            return Mathf.FloorToInt((baseValue + diceValue + contextValue) * multiplier);
        }
    }
}
