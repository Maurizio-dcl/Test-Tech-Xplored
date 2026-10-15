using TestTechXplored.Core.Scripts.Actors;
using TestTechXplored.Core.Scripts.Board;
using TestTechXplored.Game.Scripts.Definitions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TestTechXplored.ToBeKilled.Scripts.Bootstrap
{
    public class GameManager : MonoBehaviour
    {
        private Board _board;
        private BoardEntityView _selectedEntity;
        private BoardEntityView[] _entityViews;
        private CellView[,] _cellViews;
        private Transform _boardEntitiesContainer;

        [SerializeField] private GameDefinition gameDefinition;
        [SerializeField] private CellView cellViewPrefab;
        [SerializeField] private BoardMapView mapView;
        [SerializeField] private BoardEntitiesView boardEntitiesView;

        [SerializeField] private Color evenCellColor = Color.antiqueWhite;
        [SerializeField] private Color oddCellColor = Color.lightSlateGray;

        void Start()
        {
            _board = Board.Instance;
            _board.Added += OnBoardEntityAdded;
            _board.Removed += OnBoardEntityRemoved;
            SpawnBoardEntities();
            RenderBoard();
        }

        void OnDestroy()
        {
            _board.Added -= OnBoardEntityAdded;
            _board.Removed -= OnBoardEntityRemoved;
        }

        void Update()
        {
            if (_selectedEntity != null && Mouse.current != null)
                _selectedEntity.transform.position = Mouse.current.position.ReadValue();
        }

        void SpawnBoardEntities()
        {
            BoardEntitiesView view = Instantiate(boardEntitiesView, mapView.transform);
            _boardEntitiesContainer = view.EntitiesContainer;
            _entityViews = new BoardEntityView[gameDefinition.BoardEntities.Count];
            mapView.GetComponent<Button>().onClick.AddListener(
                ReturnSelectedEntity);

            for (var i = 0; i < gameDefinition.BoardEntities.Count; i++)
            {
                var entity = gameDefinition.BoardEntities[i];
                GameObject model = Instantiate(entity.Model, _boardEntitiesContainer);
                BoardEntityView entityView = model.GetComponent<BoardEntityView>();
                entityView.Configure(entity);
                _entityViews[i] = entityView;
                model.GetComponent<Button>().onClick.AddListener(
                    () => SelectEntity(entityView));
            }
        }

        void SelectEntity(BoardEntityView entityView)
        {
            if (_selectedEntity != null)
                return;

            _selectedEntity = entityView;
            BoardCell cell = _board.FindCell(entityView.Entity);

            if (cell != null)
                _board.Remove(entityView.Entity, cell);

            MoveEntityView(entityView, mapView.transform);
            entityView.RestoreSize();
            entityView.SetRaycastTargets(false);
        }

        void MoveSelectedEntity(BoardCell cell)
        {
            if (_selectedEntity == null)
                return;

            _board.Add(_selectedEntity.Entity, cell);
            ReleaseSelectedEntity();
        }

        void ReturnSelectedEntity()
        {
            if (_selectedEntity == null)
                return;

            MoveEntityView(_selectedEntity, _boardEntitiesContainer);
            ReleaseSelectedEntity();
        }

        void ReleaseSelectedEntity()
        {
            _selectedEntity.SetRaycastTargets(true);
            _selectedEntity = null;
        }

        void MoveEntityView(BoardEntityView entityView, Transform parent)
        {
            entityView.transform.SetParent(parent, false);
            entityView.transform.localPosition = Vector3.zero;
        }

        void OnBoardEntityAdded(ActorDefinition entity, BoardCell cell)
        {
            BoardEntityView entityView = FindEntityView(entity);
            CellView cellView = _cellViews[cell.Position.y, cell.Position.x];
            MoveEntityView(entityView, cellView.EntitiesContainer);

            // TODO remove after tests
            Debug.Log($"Added '{entity.name}' to {cell.Position}.");
        }

        void OnBoardEntityRemoved(ActorDefinition entity, BoardCell cell)
        {
            BoardEntityView entityView = FindEntityView(entity);
            MoveEntityView(entityView, _boardEntitiesContainer);

            // TODO remove after tests
            Debug.Log($"Removed '{entity.name}' from {cell.Position}.");
        }

        BoardEntityView FindEntityView(ActorDefinition entity)
        {
            for (int i = 0; i < _entityViews.Length; i++)
            {
                if (_entityViews[i].Entity == entity)
                    return _entityViews[i];
            }

            return null;
        }

        void RenderBoard()
        {
            mapView.Configure(_board);
            _cellViews = new CellView[_board.Height, _board.Width];

            for (int y = 0; y < _board.Height; y++)
            {
                for (int x = 0; x < _board.Width; x++)
                {
                    BoardCell cell = _board[x, y];

                    CellView view = Instantiate(cellViewPrefab, mapView.CellsContainer);
                    _cellViews[y, x] = view;
                    view.LabelText = $"({cell.Position.x}, {cell.Position.y})";
                    view.ForegroundImageColor = (x + y) % 2 == 0 ? evenCellColor : oddCellColor;
                    view.GetComponent<Button>().onClick.AddListener(
                        () => MoveSelectedEntity(cell));
                }
            }
        }
    }
}
