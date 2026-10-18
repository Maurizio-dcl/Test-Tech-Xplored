using UnityEngine;
using Game.Phases.Action;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "PushActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/Actions/PushActionDefinition")]
    public sealed class PushActionDefinition : ActionDefinition
    {
        [SerializeField] private int pushDistance;
        
        public override ActionOutcome Execute(ActionPhaseContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
