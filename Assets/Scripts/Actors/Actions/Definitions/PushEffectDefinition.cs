using System;
using UnityEngine;
using Utils;

namespace Actors.Actions.Definitions
{
    [Serializable]
    public sealed class PushEffectDefinition : ActionEffectDefinition
    {
        public PushEffectDefinition()
        {
        }

        public PushEffectDefinition(
            int baseValue,
            bool hasBonusDice = false,
            DiceExpression bonusDice = default,
            float multiplier = 1f,
            EffectValueProvider contextualValue = null)
            : base(baseValue, hasBonusDice, bonusDice, multiplier, contextualValue)
        {
        }

        public override void Execute(ActionEffectExecutionContext context)
        {
            int distance = Mathf.Max(0, ResolveValue(context));
            if (distance == 0 || !context.Map.TryGetActorTile(context.Source, out var sourceTile))
                return;

            for (int i = 0; i < context.Targets.Count; i++)
            {
                Actor target = context.Targets[i];
                if (!context.Map.TryGetActorTile(target, out var targetTile))
                    continue;

                Vector2 direction = ((Vector2)(targetTile.Position - sourceTile.Position)).normalized;
                Vector2Int displacement = Vector2Int.RoundToInt(direction * distance);
                if (displacement == Vector2Int.zero)
                    continue;

                Vector2Int origin = targetTile.Position;
                Vector2Int destination = origin + displacement;
                if (context.Map.Move(target, destination))
                    context.AddOutcome(new PushEffect(target, origin, destination));
            }
        }
    }
}