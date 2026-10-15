using UnityEngine;
using UnityEngine.UI;

namespace Map.Views
{
    public sealed class TileView : MonoBehaviour
    {
        [SerializeField] private Image foregroundImage;

        public Color ForegroundImageColor
        {
            get => foregroundImage.color;
            set => foregroundImage.color = value;
        }
    }
}
