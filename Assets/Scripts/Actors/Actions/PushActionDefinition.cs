using UnityEngine;

namespace Actors.Actions
{
    public sealed class PushActionDefinition : ActionDefinition
    {
        [SerializeField] private int pushAmount;
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}