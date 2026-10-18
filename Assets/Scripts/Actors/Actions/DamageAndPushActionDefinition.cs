using System.Collections.Generic;
using Actors;
using UnityEngine;
using Game.Phases.Action;
using Utils;
using static Actors.StatDefinition;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "DamageAndPushActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/Actions/DamageAndPushActionDefinition")]
    public sealed class DamageAndPushActionDefinition : ActionDefinition
    {
        [SerializeField] private int baseDamage = 1;
        
        [SerializeField] private bool hasDamageBonusDiceExpression = false;
        [SerializeField] private DiceExpression bonusDamageDiceExpression;
        
        [SerializeField] private int pushDistance = 0;

        [SerializeField] private bool hasPushBonusDiceExpression = false;
        [SerializeField] private DiceExpression bonusPushDiceExpression;

        private ActionPhaseContext _context;
        
        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            // Given this type of action at this point I would go about having actions composable with 
            // atomic effects themselves rather than composing them this way.
            // So that you could have damage + heal, heal + push, damage + heal + push etc.
            // I have no time left to introduce this at this point, but it is easily doable with this architecture.
            
            _context = context;

            List<IActionEffect> effects = ExecuteDamage();
            effects.AddRange(ExecutePush());

            // TODO aggro as well

            return new ActionOutcome(
                context.Turn.Round,
                context.Turn.Actor,
                context.SelectedAction,
                effects);
        }

        private List<IActionEffect> ExecuteDamage()
        {
            int bonusDamage = hasDamageBonusDiceExpression
                ? DiceRoller.Roll(bonusDamageDiceExpression).Total
                : 0;
            
            int damage = baseDamage + bonusDamage;
            
            IReadOnlyList<Actor> targets = _context.SelectedTargets;
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
            
            return effects;
        }
        
        private List<IActionEffect> ExecutePush()
        {
            int bonusPushDistance = hasPushBonusDiceExpression
                ? DiceRoller.Roll(bonusPushDiceExpression).Total
                : 0;
            int totalPushDistance = Mathf.Max(0, pushDistance + bonusPushDistance);

            IReadOnlyList<Actor> targets = _context.SelectedTargets;
            List<IActionEffect> effects = new();

            if (totalPushDistance > 0 &&
                _context.Turn.Map.TryGetActorTile(_context.Turn.Actor, out var actorTile))
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    Actor target = targets[i];
                    if (!_context.Turn.Map.TryGetActorTile(target, out var targetTile))
                        continue;

                    Vector2 pushDirection = ((Vector2)(targetTile.Position - actorTile.Position)).normalized;
                    Vector2Int displacement = Vector2Int.RoundToInt(pushDirection * totalPushDistance);

                    if (displacement == Vector2Int.zero)
                        continue;

                    Vector2Int destination = targetTile.Position + displacement;
                    if (_context.Turn.Map.Move(target, destination))
                        effects.Add(new PushEffect(target, targetTile.Position, destination));
                }
            }
            
            return effects;
        }
    }
}
