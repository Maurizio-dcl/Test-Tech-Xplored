using TestTechXplored.Core.Scripts.TagMatching;
using UnityEngine;

namespace TestTechXplored.Game.Scripts.Definitions
{
    [CreateAssetMenu(
        fileName = "GameEntityDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Game/GameEntityDefinition")]
    public sealed class GameEntityDefinition : ScriptableObject
    {
        [SerializeField]
        private string id;

        [SerializeField]
        private TagMatchingRule matchingRule;

        [SerializeField]
        private GameObject graphics;

        public string Id => id;

        public TagMatchingRule MatchingRule => matchingRule;

        public GameObject Graphics => graphics;
    }
}
