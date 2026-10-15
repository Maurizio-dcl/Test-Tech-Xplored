using TestTechXplored.Core.Scripts.Actors;
using UnityEngine;
using UnityEngine.UI;

namespace TestTechXplored.Core.Scripts.Board
{
    public sealed class BoardEntityView : MonoBehaviour
    {
        private Graphic[] _graphics;
        private RectTransform _rectTransform;
        private Vector2 _defaultSize;

        public ActorDefinition Entity { get; private set; }

        void Awake()
        {
            _graphics = GetComponentsInChildren<Graphic>();
            _rectTransform = (RectTransform)transform;
            _defaultSize = _rectTransform.sizeDelta;
        }

        public void Configure(ActorDefinition entity) => Entity = entity;

        public void SetRaycastTargets(bool raycastTarget)
        {
            for (int i = 0; i < _graphics.Length; i++)
                _graphics[i].raycastTarget = raycastTarget;
        }

        public void RestoreSize() => _rectTransform.sizeDelta = _defaultSize;
    }
}
