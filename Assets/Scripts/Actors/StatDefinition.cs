using UnityEngine;

namespace Actors
{
    [CreateAssetMenu(
        fileName = "StatDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/StatDefinition")]
    public sealed class StatDefinition : ScriptableObject
    {
        public enum StatTag
        {
            Movement,
            Aggro
            // ...Add here new stat labels to define new "stats"
        }

        [SerializeField] private StatTag tag;
        [SerializeField] private bool isVisible; // whether the stat is actually visible in-game

        public StatTag Tag => tag;
        public bool IsVisible => isVisible;
    }
}