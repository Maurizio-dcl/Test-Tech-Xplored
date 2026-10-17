using UnityEngine;
using Game.Phases.Action;

namespace Actors.Actions
{
    public sealed class PushActionDefinition : ActionDefinition
    {
        [SerializeField] private int pushAmount;
        
        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
