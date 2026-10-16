using UnityEngine;

namespace Actors.Actions
{
    [CreateAssetMenu(
        fileName = "ActionDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/Actions/ActionDefinition")]
    public abstract class ActionDefinition : ScriptableObject
    {
        [SerializeField] private string actionName;
        [SerializeField] private string actionDescription;
        [SerializeField] private int cooldown;
        // 0 = self, 1 = melee, 2+ = ranged
        [SerializeField] private int minRange;
        [SerializeField] private int maxRange;
        [SerializeField] private Targeting targeting;
        [SerializeField] private GameObject visuals;

        public int Cooldown => cooldown;

        // TODO pass down action context to Execute (coming from system)
        public abstract void Execute();
    }
}