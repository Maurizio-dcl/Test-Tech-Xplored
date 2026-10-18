namespace Actors.Actions
{
    public sealed class HealEffect : IActionEffect
    {
        public Actor Target { get; }
        public int Amount { get; }

        public HealEffect(Actor target, int amount)
        {
            Target = target;
            Amount = amount;
        }

        public string Format() =>
            $"healed {Target.Definition.GivenName} [id: {Target.UniqueId}] for {Amount}";
    }
}