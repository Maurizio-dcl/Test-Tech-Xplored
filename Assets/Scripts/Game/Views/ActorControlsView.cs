using System;
using Actors;
using UnityEngine;
using UnityEngine.UI;
using ActorAction = Actors.Actions.Action;

namespace Game.Views
{
    public sealed class ActorControlsView : MonoBehaviour
    {
        [SerializeField] private ActorStatsView actorStatsView;
        [SerializeField] private ActionControlsView actionControlsView;
        
        [SerializeField] private Button endMovementPhaseButton;

        [SerializeField] private Button endActionPhaseButton;
        [SerializeField] private Button cancelActionButton;
        [SerializeField] private Button confirmTargetsButton;

        public event Action EndMovementPhaseRequested;

        public event Action EndActionPhaseRequested;
        public event Action CancelActionRequested;
        public event Action ConfirmTargetsRequested;
        public event Action<ActorAction> ActionRequested;

        private void Awake()
        {
            actionControlsView.ActionRequested += OnActionRequested;
            
            endMovementPhaseButton.onClick.AddListener(OnEndMovementPhaseClicked);
            SetEndMovementAvailable(false);

            endActionPhaseButton.onClick.AddListener(OnEndActionPhaseClicked);
            SetEndActionAvailable(false);
            cancelActionButton.onClick.AddListener(OnCancelActionClicked);
            SetCancelActionAvailable(false);
            confirmTargetsButton.onClick.AddListener(OnConfirmTargetsClicked);
            SetConfirmTargetsAvailable(false);
        }

        private void OnDestroy()
        {
            actionControlsView.ActionRequested -= OnActionRequested;
            
            endMovementPhaseButton.onClick.RemoveListener(OnEndMovementPhaseClicked);

            endActionPhaseButton.onClick.RemoveListener(OnEndActionPhaseClicked);
            cancelActionButton.onClick.RemoveListener(OnCancelActionClicked);
            confirmTargetsButton.onClick.RemoveListener(OnConfirmTargetsClicked);
        }

        public void SetupStats(Actor actor) => actorStatsView.Setup(actor);

        public void SetupActions(Actor actor) => actionControlsView.Setup(actor);

        public void RefreshActions() => actionControlsView.Refresh();

        public void ResetActionDescription() => actionControlsView.ResetActionDescription();

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

        public void SetConfirmTargetsAvailable(bool available)
        {
            confirmTargetsButton.interactable = available;
            confirmTargetsButton.gameObject.SetActive(available);
        }

        private void OnEndMovementPhaseClicked() => EndMovementPhaseRequested?.Invoke();
        private void OnEndActionPhaseClicked() => EndActionPhaseRequested?.Invoke();
        private void OnCancelActionClicked() => CancelActionRequested?.Invoke();
        private void OnConfirmTargetsClicked() => ConfirmTargetsRequested?.Invoke();
        private void OnActionRequested(ActorAction action) => ActionRequested?.Invoke(action);
    }
}
