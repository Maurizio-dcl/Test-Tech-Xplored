using System.Collections.Generic;
using Actors.Actions;
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

        [SerializeField] private bool hasBlockingType;
        [SerializeField] private CharacterType blockingType;

        [SerializeField] private List<StatConfiguration> statConfigurations = new();
        
        [SerializeField] private List<ActionDefinition> actionDefinitions = new();

        [SerializeField] private GameObject visuals;

        public string GivenName => givenName;
        public CharacterType CharacterType => characterType;
        public CharacterType? BlockingType => hasBlockingType ? blockingType : null;
        public IReadOnlyList<StatConfiguration> StatConfigurations => statConfigurations;
        public IReadOnlyList<ActionDefinition> ActionDefinitions => actionDefinitions;
        public GameObject Visuals => visuals;
    }
}
