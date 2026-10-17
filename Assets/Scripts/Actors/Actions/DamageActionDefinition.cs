using UnityEngine;
using Game.Phases.Action;

namespace Actors.Actions
{
    public sealed class DamageActionDefinition : ActionDefinition
    {
        [SerializeField] private int baseDamage;
        public override void Execute(ActionPhaseContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
