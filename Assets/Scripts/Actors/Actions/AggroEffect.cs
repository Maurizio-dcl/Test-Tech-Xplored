namespace Actors.Actions
{
    public sealed class AggroEffect : IActionEffect
    {
        public Actor Target { get; }
        public int Amount { get; }

        public AggroEffect(Actor target, int amount)
        {
            Target = target;
            Amount = amount;
        }

        public string Format() =>
            $"modified {Target.Definition.GivenName} [id: {Target.UniqueId}] aggro by {Amount}";
    }
}
