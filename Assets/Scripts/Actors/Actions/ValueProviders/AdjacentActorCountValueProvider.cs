using System;
using UnityEngine;

namespace Actors.Actions.ValueProviders
{
    [Serializable]
    public sealed class AdjacentActorCountValueProvider : EffectValueProvider
    {
        [SerializeField] private int radius = 1;
        [SerializeField] private bool hasTypeFilter = true;
        [SerializeField] private CharacterType typeFilter = CharacterType.Hero;

        public CharacterType? TypeFilter => hasTypeFilter ? typeFilter : null;

        public AdjacentActorCountValueProvider()
        {
        }

        public AdjacentActorCountValueProvider(
            int radius,
            bool hasTypeFilter,
            CharacterType typeFilter)
        {
            this.radius = radius;
            this.hasTypeFilter = hasTypeFilter;
            this.typeFilter = typeFilter;
        }

        public override int GetValue(ActionEffectExecutionContext context) =>
            context.Map.GetActorsInRadius(context.Source, radius, TypeFilter).Count;
    }
}
