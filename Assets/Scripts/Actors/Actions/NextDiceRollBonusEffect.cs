namespace Actors.Actions
{
    public sealed class NextDiceRollBonusEffect : IActionEffect
    {
        public Actor Target { get; }
        public int Amount { get; }

        public NextDiceRollBonusEffect(Actor target, int amount)
        {
            Target = target;
            Amount = amount;
        }

        public string Format() =>
            $"granted {Target.Definition.GivenName} [id: {Target.UniqueId}] +{Amount} to their next dice roll";
    }
}
