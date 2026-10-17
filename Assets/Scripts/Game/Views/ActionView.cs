using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Action = Actors.Actions.Action;

namespace Game.Views
{
    public sealed class ActionView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image buttonImage;
        
        [SerializeField] private Image iconImage;
        
        [SerializeField] private GameObject cooldownOverlay;
        [SerializeField] private TMP_Text cooldownText;

        private Action _action;
        
        public event Action<Action> ActionRequested;
        
        public void Setup(Action action)
        {
            _action = action;
            Refresh();
        }

        private void Awake() => button.onClick.AddListener(OnButtonClick);

        private void OnDestroy() => button.onClick.RemoveListener(OnButtonClick);

        private void Refresh()
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
        }

        private void OnButtonClick() => ActionRequested?.Invoke(_action);
    }
}
