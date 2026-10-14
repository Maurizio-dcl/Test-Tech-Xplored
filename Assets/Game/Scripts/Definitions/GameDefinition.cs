using System.Collections.Generic;
using TestTechXplored.Core.Scripts.Board;
using UnityEngine;

namespace TestTechXplored.Game.Scripts.Definitions
{
    [CreateAssetMenu(
        fileName = "GameDefinition",
        menuName = "ScriptableObjects/TestTechXplored/Game/GameDefinition")]
    public sealed class GameDefinition : ScriptableObject
    {
        [SerializeField]
        private List<BoardEntityDefinition> boardEntities = new();

        [SerializeField]
        private List<GameEntityDefinition> gameEntities = new();

        public IReadOnlyList<BoardEntityDefinition> BoardEntities => boardEntities;

        public IReadOnlyList<GameEntityDefinition> GameEntities => gameEntities;
    }
}