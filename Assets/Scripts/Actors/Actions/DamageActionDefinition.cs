using UnityEngine;

namespace Actors.Actions
{
    public sealed class DamageActionDefinition : ActionDefinition
    {
        [SerializeField] private int baseDamage;
        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}