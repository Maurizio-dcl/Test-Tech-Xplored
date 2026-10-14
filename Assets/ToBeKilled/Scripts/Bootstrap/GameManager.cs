using TestTechXplored.Core.Scripts.Board;
using TestTechXplored.Game.Scripts.Definitions;
using UnityEngine;
using UnityEngine.UI;

namespace TestTechXplored.ToBeKilled.Scripts.Bootstrap
{
    public class GameManager : MonoBehaviour
    {
        private Board _board;
        private Transform _selectedEntity;
        private Transform _boardEntitiesContainer;

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
            _boardEntitiesContainer = view.EntitiesContainer;
            mapView.GetComponent<Button>().onClick.AddListener(
                () => MoveSelectedEntity(_boardEntitiesContainer));

            for (var i = 0; i < gameDefinition.BoardEntities.Count; i++)
            {
                var entity = gameDefinition.BoardEntities[i];
                GameObject model = Instantiate(entity.Model, _boardEntitiesContainer);
                model.GetComponent<Button>().onClick.AddListener(
                    () => _selectedEntity = model.transform);
            }
        }

        void MoveSelectedEntity(Transform parent)
        {
            if (_selectedEntity == null)
                return;

            if (_selectedEntity.parent == parent)
            {
                _selectedEntity = null;
                return;
            }

            _selectedEntity.SetParent(parent, false);
            _selectedEntity.localPosition = Vector3.zero;
            _selectedEntity = null;
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
                    prefab.GetComponent<Button>().onClick.AddListener(
                        () => MoveSelectedEntity(prefab.transform));
                }
            }
        }
    }
}
