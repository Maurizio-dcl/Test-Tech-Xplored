using UnityEngine;
using UnityEngine.UI;

namespace TestTechXplored.Core.Scripts.Board
{
    public sealed class BoardEntityView : MonoBehaviour
    {
        private Graphic[] _graphics;

        public BoardEntityDefinition Entity { get; private set; }

        void Awake() => _graphics = GetComponentsInChildren<Graphic>();

        public void Configure(BoardEntityDefinition entity) => Entity = entity;

        public void SetRaycastTargets(bool raycastTarget)
        {
            for (int i = 0; i < _graphics.Length; i++)
                _graphics[i].raycastTarget = raycastTarget;
        }
    }
}
