using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Action = Actors.Actions.Action;

namespace Game.Views
{
    public sealed class ActionView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Button button;
        [SerializeField] private Image buttonImage;
        
        [SerializeField] private Image iconImage;
        
        [SerializeField] private GameObject cooldownOverlay;
        [SerializeField] private TMP_Text cooldownText;

        private Action _action;
        private bool _isHovered;
        
        public event Action<Action> ActionRequested;
        public event Action<Action> ActionHovered;
        public event Action<Action> ActionUnhovered;
        
        public void Setup(Action action)
        {
            _action = action;
            Refresh();
        }

        private void Awake() => button.onClick.AddListener(OnButtonClick);

        private void OnDestroy() => button.onClick.RemoveListener(OnButtonClick);

        public void Refresh()
        {
            buttonImage.color = _action.Definition.BackgroundColor;
            iconImage.sprite = _action.Definition.Icon;

            SetAvailable(_action.IsAvailable);
        }

        private void SetAvailable(bool isAvailable)
        {
            button.interactable = isAvailable;
            cooldownOverlay.SetActive(!isAvailable);
            cooldownText.text = _action.CooldownValue.ToString();

            if (!isAvailable && _isHovered)
            {
                _isHovered = false;
                ActionUnhovered?.Invoke(_action);
            }
        }

        private void OnButtonClick() => ActionRequested?.Invoke(_action);

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!button.interactable)
                return;

            _isHovered = true;
            ActionHovered?.Invoke(_action);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isHovered)
                return;

            _isHovered = false;
            ActionUnhovered?.Invoke(_action);
        }
    }
}
