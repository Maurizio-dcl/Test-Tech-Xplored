using TestTechXplored.Core.Scripts.Board;
using TestTechXplored.Game.Scripts.Presentation;
using UnityEngine;

namespace TestTechXplored.ToBeKilled.Scripts.Bootstrap
{
    public class GameManager : MonoBehaviour
    {
        private Board _board;
        
        [SerializeField] private CellPrefab cellPrefab;
        [SerializeField] private BoardMapView mapView;
        
        [SerializeField] private Color evenCellColor = Color.antiqueWhite;
        [SerializeField] private Color oddCellColor = Color.lightSlateGray;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Init();
            UpdateRender();
        }

        void Init()
        {
            _board = Board.Instance;
        }

        void UpdateRender()
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
