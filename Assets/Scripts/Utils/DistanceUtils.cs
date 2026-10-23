using UnityEngine;

namespace Utils
{
    public static class DistanceUtils
    {
        public static int ManhattanDistance(Vector2Int a, Vector2Int b) =>
            Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
