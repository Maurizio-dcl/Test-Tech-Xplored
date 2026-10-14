using UnityEngine;

namespace TestTechXplored.Core.Scripts.Board
{
    public class Board
    {
        private static Board _instance;
        public static Board Instance => _instance ??= new Board();

        private readonly BoardCell[,] _cells;
        
        public int Width => _cells.GetLength(1);
        public int Height => _cells.GetLength(0);

        public BoardCell this[int x, int y] => _cells[y, x];

        private Board()
        {
            _cells = new BoardCell[5, 8];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector2Int position = new(x, y);
                    _cells[y, x] = new BoardCell(position);
                }
            }
        }

        public void Add(BoardEntityDefinition entity, BoardCell cell) =>
            _cells[cell.Position.y, cell.Position.x].Add(entity);
        
        public bool Remove(BoardEntityDefinition entity, BoardCell cell) =>
            _cells[cell.Position.y, cell.Position.x].Remove(entity);
    }
}
