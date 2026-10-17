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
        [SerializeField] private int baseDamage;
        [SerializeField] private Die bonusDamageDie;

        public override void Execute(ActionPhaseContext context)
        {
            int bonusDamage = DiceRoller.Roll(new DiceExpression(1, bonusDamageDie)).Total;
            int damage = baseDamage + bonusDamage;
            IReadOnlyList<Actor> targets = context.SelectedTargets;

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].TryGetStat(StatTag.Hp, out Stat hp))
                    hp.Modify(-damage);
            }
            
            // TODO aggro as well
        }
    }
}
