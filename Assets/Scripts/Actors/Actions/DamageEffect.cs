using Actors;

namespace Actors.Actions
{
    public sealed class DamageEffect : IActionEffect
    {
        public Actor Target { get; }
        public int Amount { get; }

        public DamageEffect(Actor target, int amount)
        {
            Target = target;
            Amount = amount;
        }

        public string Format() =>
            $"damaged {Target.Definition.GivenName} [id: {Target.UniqueId}] for {Amount}";
    }
}
