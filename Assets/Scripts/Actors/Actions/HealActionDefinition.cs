using UnityEngine;
using Game.Phases.Action;

namespace Actors.Actions
{
    public sealed class HealActionDefinition : ActionDefinition
    {
        [SerializeField] private int baseHeal;
        public override void Execute(ActionPhaseContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
