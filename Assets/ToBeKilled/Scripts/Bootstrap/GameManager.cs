using TestTechXplored.Core.Scripts.Board;
using TestTechXplored.Game.Scripts.Definitions;
using UnityEngine;

namespace TestTechXplored.ToBeKilled.Scripts.Bootstrap
{
    public class GameManager : MonoBehaviour
    {
        private Board _board;

        [SerializeField] private GameDefinition gameDefinition;
        [SerializeField] private CellPrefab cellPrefab;
        [SerializeField] private BoardMapView mapView;
        [SerializeField] private BoardEntitiesView boardEntitiesView;

        [SerializeField] private Color evenCellColor = Color.antiqueWhite;
        [SerializeField] private Color oddCellColor = Color.lightSlateGray;

        void Start()
        {
            _board = Board.Instance;
            SpawnBoardEntities();
            RenderBoard();
        }

        void SpawnBoardEntities()
        {
            BoardEntitiesView view = Instantiate(boardEntitiesView, mapView.transform);

            for (var i = 0; i < gameDefinition.BoardEntities.Count; i++)
            {
                var entity = gameDefinition.BoardEntities[i];
                Instantiate(entity.Model, view.EntitiesContainer);
            }
        }

        void RenderBoard()
        {
            mapView.Configure(_board);

            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    BoardCell cell = _board[x, y];

                    CellPrefab prefab = Instantiate(cellPrefab, mapView.CellsContainer);
                    prefab.LabelText = $"({cell.Position.x}, {cell.Position.y})";
                    prefab.ForegroundImageColor = (x + y) % 2 == 0 ? evenCellColor : oddCellColor;
                }
            }
        }
    }
}
