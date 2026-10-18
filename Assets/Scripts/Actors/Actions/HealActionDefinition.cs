using System.Collections.Generic;
using UnityEngine;
using Game.Phases.Action;
using Utils;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "HealActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/Actions/HealActionDefinition")]
    public sealed class HealActionDefinition : ActionDefinition
    {
        [SerializeField] private int baseHeal = 1;

        [SerializeField] private bool hasBonusDiceExpression = false;
        [SerializeField] private DiceExpression bonusHealDiceExpression;

        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            int bonusHeal = hasBonusDiceExpression
                ? DiceRoller.Roll(bonusHealDiceExpression).Total
                : 0;
            
            int heal = baseHeal + bonusHeal;
            
            IReadOnlyList<Actor> targets = context.SelectedTargets;
            List<IActionEffect> effects = new();
            
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].TryGetStat(StatDefinition.StatTag.Hp, out Stat hp))
                {
                    int hpBeforeHeal = hp.Value;
                    hp.Modify(heal);
                    int appliedHeal = hp.Value - hpBeforeHeal;

                    effects.Add(new HealEffect(targets[i], appliedHeal));
                }
            }
            
            // TODO aggro as well
            
            return new ActionOutcome(
                context.Turn.Round,
                context.Turn.Actor,
                context.SelectedAction,
                effects);
        }
    }
}