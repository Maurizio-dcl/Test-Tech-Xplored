using System;
using UnityEngine;
using Utils;

namespace Actors.Actions
{
    [Serializable]
    public struct ActionNumericalValues
    {
        [SerializeField] private int[] fixedValues;
        [SerializeField] private DiceExpression[] diceExpressionValues;

        public ReadOnlySpan<int> FixedValues => fixedValues;
        public ReadOnlySpan<DiceExpression> DiceExpressionValues => diceExpressionValues;
        
        public ActionNumericalValues(int[] fixedValues, DiceExpression[] diceExpressionValues)
        {
            this.fixedValues = fixedValues;
            this.diceExpressionValues = diceExpressionValues;
        }
    }
}