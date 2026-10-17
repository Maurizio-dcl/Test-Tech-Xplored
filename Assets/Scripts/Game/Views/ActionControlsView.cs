using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public sealed class ActionControlsView : MonoBehaviour
    {
        
    }

    public sealed class ActorControlsView : MonoBehaviour
    {
        [SerializeField] private Button endMovementPhaseButton;

        [SerializeField] private Button endActionPhaseButton;
        [SerializeField] private Button cancelActionButton;

        public event Action EndMovementPhaseRequested;

        public event Action EndActionPhaseRequested;
        public event Action CancelActionRequested;

        private void Awake()
        {
            endMovementPhaseButton.onClick.AddListener(OnEndMovementPhaseClicked);
            SetEndMovementAvailable(false);
            
            endActionPhaseButton.onClick.AddListener(OnEndActionPhaseClicked);
            SetEndActionAvailable(false);
            cancelActionButton.onClick.AddListener(OnCancelActionClicked);
            SetCancelActionAvailable(false);
        }

        private void OnDestroy() {
            endMovementPhaseButton.onClick.RemoveListener(OnEndMovementPhaseClicked);
            
            endActionPhaseButton.onClick.RemoveListener(OnEndActionPhaseClicked);
            cancelActionButton.onClick.RemoveListener(OnCancelActionClicked);
        }

        public void SetEndMovementAvailable(bool available)
        {
            endMovementPhaseButton.interactable = available;
            endMovementPhaseButton.gameObject.SetActive(available);
        }
        
        public void SetEndActionAvailable(bool available)
        {
            endActionPhaseButton.interactable = available;
            endActionPhaseButton.gameObject.SetActive(available);
        }

        public void SetCancelActionAvailable(bool available)
        {
            cancelActionButton.interactable = available;
            cancelActionButton.gameObject.SetActive(available);
        }

        private void OnEndMovementPhaseClicked() => EndMovementPhaseRequested?.Invoke();
        private void OnEndActionPhaseClicked() => EndActionPhaseRequested?.Invoke();
        private void OnCancelActionClicked() => CancelActionRequested?.Invoke();
    }
}