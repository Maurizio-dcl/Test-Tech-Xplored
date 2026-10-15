using UnityEngine;

namespace Actors
{
    [CreateAssetMenu(
        fileName = "StatDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/StatDefinition")]
    public sealed class StatDefinition : ScriptableObject
    {
        [SerializeField] private string statName;

        public string StatName => statName;
    }
}