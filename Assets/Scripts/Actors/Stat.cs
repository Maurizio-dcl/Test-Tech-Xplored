using System;
using UnityEngine;

namespace Actors
{
    public sealed class Stat
    {
        public event Action ValueChanged;

        public StatDefinition Definition { get; }
        public int MinValue { get; }
        public int MaxValue { get; }

        public int Value { get; private set; }

        public Stat(StatConfiguration config)
        {
            if (config.MinValue > config.MaxValue)
                throw new ArgumentException("Invalid stat range.");

            Definition = config.Definition;
            MinValue = config.MinValue;
            MaxValue = config.MaxValue;

            Value = Mathf.Clamp(
                config.InitialValue,
                MinValue,
                MaxValue);
        }

        public void RestoreToMax() => SetValue(MaxValue);

        public void Modify(int amount) => SetValue(Math.Clamp(Value + amount, MinValue, MaxValue));

        private void SetValue(int value)
        {
            int clampedValue = Mathf.Clamp(value, MinValue, MaxValue);
            if (Value == clampedValue)
                return;

            Value = clampedValue;
            ValueChanged?.Invoke();
        }
    }
}
