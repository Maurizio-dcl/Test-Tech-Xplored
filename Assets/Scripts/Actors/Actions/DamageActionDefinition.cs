using System.Collections.Generic;
using Actors;
using UnityEngine;
using Game.Phases.Action;
using Utils;
using static Actors.StatDefinition;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "DamageActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/Actions/DamageActionDefinition")]
    public sealed class DamageActionDefinition : ActionDefinition
    {
        [SerializeField] private int baseDamage = 1;
        [SerializeField] private DiceExpression bonusDamageDiceExpression = new(1, Die.D2);

        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            int bonusDamage = DiceRoller.Roll(bonusDamageDiceExpression).Total;
            int damage = baseDamage + bonusDamage;
            IReadOnlyList<Actor> targets = context.SelectedTargets;
            List<IActionEffect> effects = new();

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].TryGetStat(StatTag.Hp, out Stat hp))
                {
                    int hpBeforeDamage = hp.Value;
                    hp.Modify(-damage);
                    int appliedDamage = hpBeforeDamage - hp.Value;

                    effects.Add(new DamageEffect(targets[i], appliedDamage));
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
