using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    [CreateAssetMenu(
        fileName = "ActorDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Actor/ActorDefinition",
        order = 1)]
    public sealed class ActorDefinition : ScriptableObject
    {
        [SerializeField] private string givenName;

        [SerializeField] private CharacterType characterType;

        [SerializeField] private List<StatConfiguration> statConfigurations = new();

        [SerializeField] private GameObject visuals;

        public string GivenName => givenName;
        public CharacterType CharacterType => characterType;
        public IReadOnlyList<StatConfiguration> StatConfigurations => statConfigurations;
        public GameObject Visuals => visuals;
    }
}