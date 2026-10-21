using System;

namespace Actors.Actions
{
    [Serializable]
    public abstract class EffectValueProvider
    {
        public abstract int GetValue(ActionEffectExecutionContext context);
    }
}
