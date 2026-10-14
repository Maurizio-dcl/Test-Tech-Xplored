using System.Collections.Generic;
using UnityEngine;

namespace TestTechXplored.Core.Scripts.Board
{
    public sealed class BoardCell
    {
        private readonly List<BoardEntityDefinition> _entities;

        public Vector2Int Position { get; }

        internal BoardCell(Vector2Int position)
        {
            Position = position;
            _entities = new();
        }
        
        internal void Add(BoardEntityDefinition entity) => _entities.Add(entity);

        internal bool Remove(BoardEntityDefinition entity) => _entities.Remove(entity);

        internal bool Contains(BoardEntityDefinition entity) => _entities.Contains(entity);
    }
}
