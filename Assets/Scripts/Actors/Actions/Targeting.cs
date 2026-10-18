using System;
using UnityEngine;

namespace Actors.Actions
{
    [Serializable]
    public struct Targeting
    {
        [SerializeField] private TargetingMode mode;

        [SerializeField] private bool hasTypeFilter;
        [SerializeField] private CharacterType typeFilter;

        public Targeting(TargetingMode mode, bool hasTypeFilter, CharacterType typeFilter)
        {
            this.mode = mode;
            this.hasTypeFilter = hasTypeFilter;
            this.typeFilter = typeFilter;
        }

        public TargetingMode Mode => mode;
        public CharacterType? TypeFilter => hasTypeFilter ? typeFilter : null;
    }
}