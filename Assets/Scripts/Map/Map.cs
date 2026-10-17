using System;
using System.Collections.Generic;
using Actors;
using UnityEngine;
using UnityEngine.Pool;

namespace Map
{
    public sealed class Map
    {
        private readonly Vector2Int[] _directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        private readonly Tile[,] _tiles;
        private readonly Dictionary<Actor, Tile> _actorPositions = new();

        public event Action<Actor, Tile, Tile> ActorMoved;

        public int Width => _tiles.GetLength(1);
        public int Height => _tiles.GetLength(0);

        public Tile this[int x, int y] => _tiles[y, x];

        public Map()
        {
            _tiles = new Tile[5, 8];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _tiles[y, x] = new Tile(new Vector2Int(x, y));
                }
            }
        }

        public bool Move(Actor actor, Vector2Int position)
        {
            if (!IsInside(position))
                return false;

            Tile destinationTile = _tiles[position.y, position.x];
            return Move(actor, destinationTile);
        }

        public bool Move(Actor actor, Tile destination)
        {
            if (actor == null || destination == null)
                return false;

            Vector2Int p = destination.Position;

            if (_tiles[p.y, p.x] != destination)
                return false;

            if (destination.IsOccupied)
                return destination.Actor == actor;

            // Update the tile's actor reference as well as the dictionary
            if (_actorPositions.TryGetValue(actor, out Tile previous))
                previous.Actor = null;

            destination.Actor = actor;
            _actorPositions[actor] = destination;

            ActorMoved?.Invoke(actor, previous, destination);

            return true;
        }

        public bool TryGetActorTile(Actor actor, out Tile tile)
        {
            if (actor == null)
            {
                tile = null;
                return false;
            }

            return _actorPositions.TryGetValue(actor, out tile);
        }

        // Naive search, I think this is more than enough for this example, could be replaced with a flood-fill like
        // GetTilesInRange to have it scale better if the number of actors increase
        public Actor FindClosestActor(
            Actor originActor,
            CharacterType? typeFilter = null)
        {
            if (!_actorPositions.TryGetValue(originActor, out Tile originTile))
                return null;

            Actor closest = null;
            int closestDistance = int.MaxValue;

            foreach (var pair in _actorPositions)
            {
                Actor candidate = pair.Key;

                // Skip if it's me
                if (candidate == originActor)
                    continue;

                // Skip if we have a filter and does not match
                if (typeFilter.HasValue && candidate.Definition.CharacterType != typeFilter.Value)
                    continue;

                int distance = ManhattanDistance(
                    originTile.Position,
                    pair.Value.Position);

                // Skip if it's farther away than best
                if (distance >= closestDistance)
                    continue;

                closestDistance = distance;
                closest = candidate;
            }

            return closest;
        }

        // Naive search as well, could be improved in the same manner as FindClosestActor
        public List<Actor> GetActorsInRadius(
            Actor originActor,
            int radius,
            CharacterType? typeFilter = null)
        {
            var result = new List<Actor>();

            if (radius < 0 || !_actorPositions.TryGetValue(originActor, out Tile originTile))
                return result;

            foreach (var pair in _actorPositions)
            {
                Actor candidate = pair.Key;

                // Skip if it's me
                if (candidate == originActor)
                    continue;

                // Skip if we have a filter and does not match
                if (typeFilter.HasValue && candidate.Definition.CharacterType != typeFilter.Value)
                    continue;

                if (ManhattanDistance(originTile.Position, pair.Value.Position) <= radius)
                    result.Add(candidate);
            }

            return result;
        }

        // This uses a flood-fill algorithm to evaluate neighboring tiles until maxDistance is reached.
        // Blocking types stop the search, while excluded types are traversed but omitted from the result.
        public Dictionary<Tile, int> GetTilesInRange(
            Actor actor,
            int minDistance,
            int maxDistance,
            CharacterType? blockingType = null,
            List<CharacterType> excludedTypes = null)
        {
            var result = new Dictionary<Tile, int>();

            // If the range is invalid or the actor is not on the map there are no tiles to search
            if (minDistance < 0 || maxDistance < minDistance ||
                !_actorPositions.TryGetValue(actor, out Tile start))
                return result;

            // New discovered tiles whose neighbors will be searched in the loop
            var pending = ListPool<Tile>.Get();
            // This is used to track visited tiles
            var distances = DictionaryPool<Tile, int>.Get();

            try
            {
                pending.Add(start);
                distances.Add(start, 0);

                // The actor tile itself is in range when the minimum distance is zero
                // unless its type was explicitly excluded from the results
                if (minDistance == 0 && !IsExcluded(start, excludedTypes))
                    result.Add(start, 0);

                // For each available tile we perform a breadth-first search in the 4 axis
                for (int i = 0; i < pending.Count; i++)
                {
                    Tile current = pending[i];
                    int distance = distances[current];

                    // if this tile is already at max search distance do not search further
                    if (distance >= maxDistance)
                        continue;

                    // breadth-first search
                    foreach (Vector2Int direction in _directions)
                    {
                        Vector2Int searchPosition = current.Position + direction;

                        // Skip if outside the map
                        if (!IsInside(searchPosition))
                            continue;

                        // Tile we're looking at
                        Tile neighbor = _tiles[searchPosition.y, searchPosition.x];

                        // Skip if we already searched this tile
                        if (distances.ContainsKey(neighbor))
                            continue;

                        // We skip over searching and adding to results if the tile is occupied by an actor
                        // which type we consider blocking
                        if (neighbor.IsOccupied &&
                            blockingType != null &&
                            neighbor.Actor.Definition.CharacterType == blockingType.Value)
                            continue;

                        // Neighboring tiles are always one step farther away from the current tile
                        int neighborDistance = distance + 1;

                        // Mark as already visited
                        distances.Add(neighbor, neighborDistance);
                        // We will search this tile's neighbors next
                        pending.Add(neighbor);

                        // Do not add tiles before the minimum distance or tiles occupied by an excluded type
                        if (neighborDistance >= minDistance && !IsExcluded(neighbor, excludedTypes))
                            result.Add(neighbor, neighborDistance);
                    }
                }
            }
            finally
            {
                ListPool<Tile>.Release(pending);
                DictionaryPool<Tile, int>.Release(distances);
            }

            return result;
        }

        // Empty tiles cannot be excluded since exclusions only apply to actor types
        private static bool IsExcluded(Tile tile, List<CharacterType> excludedTypes) =>
            tile.IsOccupied &&
            excludedTypes != null &&
            excludedTypes.Contains(tile.Actor.Definition.CharacterType);

        private bool IsInside(Vector2Int position) =>
            position.x >= 0 && position.x < Width && position.y >= 0 && position.y < Height;

        private static int ManhattanDistance(Vector2Int a, Vector2Int b) =>
            Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
