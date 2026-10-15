using System;
using UnityEngine;

namespace Actors
{
    [Serializable]
    public struct StatConfiguration
    {
        [SerializeField] private StatDefinition definition;
        [SerializeField] private int minValue;
        [SerializeField] private int maxValue;
        [SerializeField] private int initialValue;

        public StatDefinition Definition => definition;
        public int MinValue => minValue;
        public int MaxValue => maxValue;
        public int InitialValue => initialValue;

        public StatConfiguration(StatDefinition definition,
            int minValue,
            int maxValue,
            int initialValue)
        {
            this.definition = definition;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.initialValue = initialValue;
        }
    }
}