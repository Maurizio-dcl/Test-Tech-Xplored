using System;

namespace Actors
{
    [Serializable]
    public readonly struct StatConfiguration
    {
        public readonly StatDefinition Definition;
        public readonly int MinValue;
        public readonly int MaxValue;
        public readonly int InitialValue;
        public readonly bool IsVisible;

        public StatConfiguration(StatDefinition definition,
            int minValue,
            int maxValue,
            int initialValue,
            bool isVisible)
        {
            Definition = definition;
            MinValue = minValue;
            MaxValue = maxValue;
            InitialValue = initialValue;
            IsVisible = isVisible;
        }
    }
}