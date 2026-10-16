using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Map.Views
{
    public sealed class TileView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image foregroundImage;

        public event Action<TileView> Clicked;

        public Color ForegroundImageColor
        {
            get => foregroundImage.color;
            set => foregroundImage.color = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                Clicked?.Invoke(this);
        }
    }
}
