using System.Collections.Generic;
using UnityEngine;
using Game.Phases.Action;
using Utils;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "PushActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/Actions/PushActionDefinition")]
    public class PushActionDefinition : ActionDefinition
    {
        [SerializeField] protected int pushDistance = 0;

        [SerializeField] protected bool hasBonusDiceExpression = false;
        [SerializeField] protected DiceExpression bonusPushDiceExpression;

        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            int bonusPushDistance = hasBonusDiceExpression
                ? DiceRoller.Roll(bonusPushDiceExpression).Total
                : 0;
            int totalPushDistance = Mathf.Max(0, pushDistance + bonusPushDistance);

            IReadOnlyList<Actor> targets = context.SelectedTargets;
            List<IActionEffect> effects = new();

            if (totalPushDistance > 0 &&
                context.Turn.Map.TryGetActorTile(context.Turn.Actor, out var actorTile))
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    Actor target = targets[i];
                    if (!context.Turn.Map.TryGetActorTile(target, out var targetTile))
                        continue;

                    Vector2 pushDirection = ((Vector2)(targetTile.Position - actorTile.Position)).normalized;
                    Vector2Int displacement = Vector2Int.RoundToInt(pushDirection * totalPushDistance);

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
