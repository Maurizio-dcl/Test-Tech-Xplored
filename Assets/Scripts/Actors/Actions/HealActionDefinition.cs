using UnityEngine;
using Game.Phases.Action;

namespace Actors.Actions
{
    public sealed class HealActionDefinition : ActionDefinition
    {
        [SerializeField] private int baseHeal;
        
        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
