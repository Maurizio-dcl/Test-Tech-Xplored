using System;
using System.Collections.Generic;
using Actors;
using UnityEngine;
using UnityEngine.UI;

namespace Map.Views
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class MapView : MonoBehaviour
    {
        [SerializeField] private RectTransform gridTransform;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private TileView tileViewPrefab;
        [SerializeField, Min(1f)] private float tileSize = 100f;
        [SerializeField, Min(0f)] private float horizontalPadding = 32f;
        [SerializeField, Min(0f)] private float verticalPadding = 24f;

        [SerializeField] private Color defaultTileForegroundColor = Color.antiqueWhite;

        private readonly Dictionary<Tile, TileView> _tileViews = new();
        private readonly Dictionary<TileView, Tile> _tilesByView = new();
        private readonly Dictionary<Actor, GameObject> _actorViews = new();
        private readonly HashSet<Tile> _highlightedTiles = new();

        private Map _map;

        public event Action<Tile> TileClicked;

        public void Initialize(Map map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            if (tileViewPrefab == null || gridTransform == null || gridLayout == null)
                throw new InvalidOperationException("MapView is missing one or more serialized references.");

            Unsubscribe();
            ClearRenderedObjects();

            _map = map;
            _map.ActorMoved += OnActorMoved;
            _map.ActorRemoved += OnActorRemoved;

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = map.Width;
            ResizeGrid();

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    Tile tile = map[x, y];
                    TileView tileView = Instantiate(tileViewPrefab, gridTransform);
                    tileView.name = $"Tile ({x}, {y})";
                    tileView.ForegroundImageColor = defaultTileForegroundColor;
                    tileView.Clicked += OnTileClicked;
                    _tileViews.Add(tile, tileView);
                    _tilesByView.Add(tileView, tile);
                }
            }
        }

        public void SetTilesColor(HashSet<Tile> tiles, Color color)
        {
            foreach (Tile tile in tiles)
            {
                SetTileColor(tile, color);
                _highlightedTiles.Add(tile);
            }
        }

        public void ClearTilesColor()
        {
            foreach (Tile tile in _highlightedTiles)
                SetTileColor(tile, defaultTileForegroundColor);

            _highlightedTiles.Clear();
        }

        private void OnDestroy() => Unsubscribe();

        private void OnRectTransformDimensionsChange() => ResizeGrid();

        private void OnActorMoved(Actor actor, Tile previous, Tile destination) => RenderActor(actor, destination);

        private void OnActorRemoved(Actor actor, Tile tile)
        {
            if (!_actorViews.TryGetValue(actor, out GameObject actorView))
                return;

            _actorViews.Remove(actor);
            Destroy(actorView);
        }

        private void OnTileClicked(TileView tileView)
        {
            if (_tilesByView.TryGetValue(tileView, out Tile tile))
                TileClicked?.Invoke(tile);
        }

        private void RenderActor(Actor actor, Tile destination)
        {
            if (!_tileViews.TryGetValue(destination, out TileView tileView))
            {
                Debug.LogWarning($"Cannot render actor {actor.UniqueId}: its destination is not part of this view.", this);
                return;
            }

            if (!_actorViews.TryGetValue(actor, out GameObject actorView))
            {
                if (actor.Definition.Visuals == null)
                {
                    Debug.LogWarning($"Cannot render actor {actor.UniqueId}: its definition has no visuals.", this);
                    return;
                }

                actorView = Instantiate(actor.Definition.Visuals);
                actorView.name = $"Actor {actor.UniqueId} - {actor.Definition.GivenName}";
                _actorViews.Add(actor, actorView);
            }

            actorView.transform.SetParent(tileView.transform, false);
            actorView.transform.localPosition = Vector3.zero;
        }

        private void SetTileColor(Tile tile, Color color)
        {
            if (_tileViews.TryGetValue(tile, out TileView tileView))
                tileView.ForegroundImageColor = color;
        }

        private void ResizeGrid()
        {
            if (_map == null || gridTransform == null || gridLayout == null)
                return;

            Rect rect = ((RectTransform)transform).rect;
            float availableWidth = Mathf.Max(0f, rect.width - horizontalPadding * 2f);
            float availableHeight = Mathf.Max(0f, rect.height - verticalPadding * 2f);
            float gridWidth = tileSize * _map.Width;
            float gridHeight = tileSize * _map.Height;
            float gridScale = Mathf.Min(availableWidth / gridWidth, availableHeight / gridHeight);

            gridLayout.cellSize = new Vector2(tileSize, tileSize);
            gridTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, gridWidth);
            gridTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gridHeight);
            gridTransform.anchoredPosition = Vector2.zero;
            gridTransform.localScale = new Vector3(gridScale, gridScale, 1f);
        }

        private void ClearRenderedObjects()
        {
            _highlightedTiles.Clear();

            foreach (GameObject actorView in _actorViews.Values)
                Destroy(actorView);

            foreach (TileView tileView in _tileViews.Values)
            {
                tileView.Clicked -= OnTileClicked;
                Destroy(tileView.gameObject);
            }

            _actorViews.Clear();
            _tileViews.Clear();
            _tilesByView.Clear();
        }

        private void Unsubscribe()
        {
            if (_map != null)
            {
                _map.ActorMoved -= OnActorMoved;
                _map.ActorRemoved -= OnActorRemoved;
            }

            _map = null;
        }
    }
}
