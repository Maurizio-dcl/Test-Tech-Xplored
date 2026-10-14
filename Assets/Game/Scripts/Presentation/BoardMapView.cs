using TestTechXplored.Core.Scripts.Board;
using UnityEngine;
using UnityEngine.UI;

namespace TestTechXplored.Game.Scripts.Presentation
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class BoardMapView : MonoBehaviour
    {
        [SerializeField] private RectTransform gridTransform;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField, Min(1f)] private float cellSize = 100f;
        [SerializeField, Min(0f)] private float horizontalPadding = 32f;
        [SerializeField, Min(0f)] private float verticalPadding = 24f;

        private int _columns;
        private int _rows;

        public Transform CellsContainer => gridTransform;

        public void Configure(Board board)
        {
            _columns = board.Width;
            _rows = board.Height;
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = _columns;
            ResizeGrid();
        }

        private void ResizeGrid()
        {
            if (_columns <= 0 || _rows <= 0 || gridTransform == null || gridLayout == null)
            {
                return;
            }

            Rect rect = ((RectTransform)transform).rect;
            float availableWidth = Mathf.Max(0f, rect.width - horizontalPadding * 2f);
            float availableHeight = Mathf.Max(0f, rect.height - verticalPadding * 2f);
            float gridWidth = cellSize * _columns;
            float gridHeight = cellSize * _rows;
            float gridScale = Mathf.Min(availableWidth / gridWidth, availableHeight / gridHeight);

            gridLayout.cellSize = new Vector2(cellSize, cellSize);
            gridTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gridWidth);
            gridTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gridHeight);
            gridTransform.anchoredPosition = Vector2.zero;
            gridTransform.localScale = new Vector3(gridScale, gridScale, 1f);
        }
    }
}
