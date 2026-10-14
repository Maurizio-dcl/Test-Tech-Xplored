using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestTechXplored.Core.Scripts.Board
{
    public enum BoardEntityTag
    {
        Category,
        Type,
        Name
    }

    [Serializable]
    public struct TagValue
    {
        public BoardEntityTag tag;
        public string value;
    }

    [CreateAssetMenu(
        fileName = "BoardEntityDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Board/BoardEntityDefinition",
        order = 1)]
    public sealed class BoardEntityDefinition : ScriptableObject
    {
        [SerializeField]
        private string uniqueId;

        [SerializeField]
        private List<TagValue> tags = new();

        [SerializeField]
        private GameObject model;

        public string UniqueId => uniqueId;

        public IReadOnlyList<TagValue> Tags => tags;

        public GameObject Model => model;
    }
}
