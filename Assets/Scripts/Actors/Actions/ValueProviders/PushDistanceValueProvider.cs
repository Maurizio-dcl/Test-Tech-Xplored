using System;

namespace Actors.Actions.ValueProviders
{
    [Serializable]
    public sealed class PushDistanceValueProvider : EffectValueProvider
    {
        public override int GetValue(ActionEffectExecutionContext context)
        {
            int total = 0;
            for (int i = 0; i < context.Outcomes.Count; i++)
            {
                if (context.Outcomes[i] is PushEffect push)
                    total += push.Distance;
            }

            return total;
        }
    }
}
