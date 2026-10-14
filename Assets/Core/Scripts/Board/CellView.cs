using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestTechXplored.Core.Scripts.Board
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image foregroundImage;
        [SerializeField] private TMP_Text label;

        public Color ForegroundImageColor
        {
            get => foregroundImage.color;
            set => foregroundImage.color = value;
        }

        public string LabelText
        {
            get => label.text;
            set => label.text = value;
        }
    }
}
