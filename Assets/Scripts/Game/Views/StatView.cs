using Actors;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public sealed class StatView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void Setup(Stat stat) =>
            text.text = $"{stat.Definition.Tag} : {stat.Value} / {stat.MaxValue}";
    }
}
