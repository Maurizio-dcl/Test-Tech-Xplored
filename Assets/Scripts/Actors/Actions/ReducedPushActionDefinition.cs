using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Phases.Action;
using UnityEditor.Rendering;
using Utils;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "ReducedPushActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/Actions/ReducedPushActionDefinition")]
    public sealed class ReducedPushActionDefinition : PushActionDefinition
    {

        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            int bonusPushDistance = hasBonusDiceExpression
                ? DiceRoller.Roll(bonusPushDiceExpression).Total
                : 0;
            int totalPushDistance = Mathf.Max(0, pushDistance + bonusPushDistance);

            // This is kinda weird, in a real case scenario I would have checked requirements with designers because:
            // 1 = 0, 2 = 1, 3 = 1, 4 = 2, 5 = 2, 6 = 3
            // So you could end up not moving the target at all and has uneven distribution,
            // but I'm following requirements exactly
            int adjustedPushDistance = Mathf.FloorToInt(totalPushDistance / 2);
            
            IReadOnlyList<Actor> targets = context.SelectedTargets;
            List<IActionEffect> effects = new();

            if (adjustedPushDistance > 0 &&
                context.Turn.Map.TryGetActorTile(context.Turn.Actor, out var actorTile))
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    Actor target = targets[i];
                    if (!context.Turn.Map.TryGetActorTile(target, out var targetTile))
                        continue;

                    Vector2 pushDirection = ((Vector2)(targetTile.Position - actorTile.Position)).normalized;
                    Vector2Int displacement = Vector2Int.RoundToInt(pushDirection * adjustedPushDistance);

                    if (displacement == Vector2Int.zero)
                        continue;

                    Vector2Int destination = targetTile.Position + displacement;
                    if (context.Turn.Map.Move(target, destination))
                        effects.Add(new PushEffect(target, targetTile.Position, destination));
                }
            }

            return new ActionOutcome(
                context.Turn.Round,
                context.Turn.Actor,
                context.SelectedAction,
                effects);
        }
    }
}
