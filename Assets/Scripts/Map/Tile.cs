using System.Collections.Generic;
using Actors;
using UnityEngine;

namespace Map
{
    public sealed class Tile
    {
        public Vector2Int Position { get; }

        public Actor Actor { get; internal set; }

        public bool IsOccupied => Actor != null;

        internal Tile(Vector2Int position)
        {
            Position = position;
        }
    }
}
