using System.Collections.Generic;
using Game.Phases.Action;
using UnityEngine;
using Utils;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "CompoundDamageActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/Actions/CompoundDamageActionDefinition")]
    public sealed class CompoundDamageActionDefinition : DamageActionDefinition
    {
        [SerializeField] private int adjacentHeroAdditionalDamage = 1;

        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            int bonusDamage = DiceRoller.Roll(bonusDamageDiceExpression).Total;
            int adjacentHeroCount = context.Turn.Map
                .GetActorsInRadius(context.Turn.Actor, 1, CharacterType.Hero)
                .Count;
            int damage = baseDamage + bonusDamage + (adjacentHeroCount * adjacentHeroAdditionalDamage);
            
            IReadOnlyList<Actor> targets = context.SelectedTargets;
            List<IActionEffect> effects = new();

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].TryGetStat(StatDefinition.StatTag.Hp, out Stat hp))
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
