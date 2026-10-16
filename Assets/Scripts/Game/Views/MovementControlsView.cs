using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public sealed class MovementControlsView : MonoBehaviour
    {
        [SerializeField] private Button endMovementButton;

        public event Action EndMovementRequested;

        private void Awake()
        {
            endMovementButton.onClick.AddListener(OnEndMovementClicked);
            SetEndMovementAvailable(false);
        }

        private void OnDestroy() => endMovementButton.onClick.RemoveListener(OnEndMovementClicked);

        public void SetEndMovementAvailable(bool available)
        {
            endMovementButton.interactable = available;
            endMovementButton.gameObject.SetActive(available);
        }

        private void OnEndMovementClicked() => EndMovementRequested?.Invoke();
    }
}
