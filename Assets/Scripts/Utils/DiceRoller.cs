using System;

namespace Utils
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
            int diceNumber = expression.DiceNumber;
            Die die = expression.Die;
            int dieFaces = (int)die;
            
            if (diceNumber <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(expression.DiceNumber),
                    expression.DiceNumber,
                    "Dice number in expression must be greater than zero.");
            
            if (dieFaces <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(expression.Die),
                    expression.Die,
                    "Die faces must be greater than zero.");
            
            DieRollResult[] results = new DieRollResult[diceNumber];
            int total = 0;

            for (int i = 0; i < results.Length; i++)
            {
                int rollResult = RollSingleDie(dieFaces);
                results[i] = new DieRollResult(die, rollResult);
                total += rollResult;
            }

            return new DiceRollResult(results, total);
        }

        private static int RollSingleDie(int faces) => UnityEngine.Random.Range(1, faces + 1);
    }
}