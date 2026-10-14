using System;

namespace TestTechXplored.Core.Scripts.Dice
{
    public readonly struct DieRollResult
    {
        public Die RolledDie { get; }
        public int RollResult { get; }

        internal DieRollResult(Die rolledDie, int rollResult)
        {
            RolledDie = rolledDie;
            RollResult = rollResult;
        }
    }

    public readonly struct DiceRollResult
    {
        private readonly DieRollResult[] _results;

        public ReadOnlySpan<DieRollResult> Results => _results;
        public int Total { get; }

        internal DiceRollResult(DieRollResult[] results, int total)
        {
            _results = results;
            Total = total;
        }
    }

    public static class DiceRoller
    {
        public static DiceRollResult Roll(DiceExpression expression)
        {
            if (expression.DiceNumber <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(expression.DiceNumber),
                    expression.DiceNumber,
                    "Dice number in expression must be greater than zero.");
            
            if (expression.Die.Faces <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(expression.Die.Faces),
                    expression.Die.Faces,
                    "Die faces must be greater than zero.");

            Die die = expression.Die;
            DieRollResult[] results = new DieRollResult[expression.DiceNumber];
            int total = 0;

            for (int i = 0; i < results.Length; i++)
            {
                int rollResult = RollSingleDie(die);
                results[i] = new DieRollResult(die, rollResult);
                total += rollResult;
            }

            return new DiceRollResult(results, total);
        }

        private static int RollSingleDie(Die die) => UnityEngine.Random.Range(1, die.Faces + 1);
    }
}