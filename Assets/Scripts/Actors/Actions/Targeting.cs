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
        
        public TargetingMode Mode => mode;
        public CharacterType? TypeFilter => hasTypeFilter ? typeFilter : null;
    }
}