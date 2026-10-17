using System;
using Actors;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public sealed class StatView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private Stat _stat;

        public void Setup(Stat stat)
        {
            // Reset events
            if (_stat != null)
                _stat.ValueChanged -= Refresh;

            _stat = stat;
            _stat.ValueChanged += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (_stat != null)
                _stat.ValueChanged -= Refresh;
        }

        private void Refresh() =>
            text.text = $"{_stat.Definition.Tag} : {_stat.Value} / {_stat.MaxValue}";
    }
}
