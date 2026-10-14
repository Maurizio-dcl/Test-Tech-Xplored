namespace TestTechXplored.Core.Scripts.Dice
{
    public readonly struct DiceExpression
    {
        public int DiceNumber { get; }
        public Die Die { get; }

        public DiceExpression(int diceNumber, Die die)
        {
            DiceNumber = diceNumber;
            Die = die;
        }
    }
}