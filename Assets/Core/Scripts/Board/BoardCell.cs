using System.Collections.Generic;
using TestTechXplored.Core.Scripts.Actors;
using UnityEngine;

namespace TestTechXplored.Core.Scripts.Board
{
    public sealed class BoardCell
    {
        private readonly List<ActorDefinition> _entities;

        public Vector2Int Position { get; }

        internal BoardCell(Vector2Int position)
        {
            Position = position;
            _entities = new();
        }
        
        internal void Add(ActorDefinition entity) => _entities.Add(entity);

        internal bool Remove(ActorDefinition entity) => _entities.Remove(entity);

        internal bool Contains(ActorDefinition entity) => _entities.Contains(entity);
    }
}
