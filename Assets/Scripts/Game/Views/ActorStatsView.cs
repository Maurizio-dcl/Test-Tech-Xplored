using System;
using System.Collections.Generic;
using Actors;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public sealed class ActorStatsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text actorName;
        [SerializeField] private StatView statViewPrefab;
        [SerializeField] private Transform statsContainer;

        private readonly List<StatView> _statViews = new();

        private void Awake() => statsContainer.GetComponentsInChildren(true, _statViews);

        public void Setup(Actor actor)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            actorName.text = actor.Definition.GivenName;

            ClearStatViews();

            foreach (Stat stat in actor.Stats)
            {
                if (!stat.Definition.IsVisible)
                    continue;

                StatView statView = Instantiate(statViewPrefab, statsContainer);
                statView.Setup(stat);
                _statViews.Add(statView);
            }
        }

        private void ClearStatViews()
        {
            for (int i = 0; i < _statViews.Count; i++)
            {
                StatView statView = _statViews[i];
                statView.gameObject.SetActive(false);
                Destroy(statView.gameObject);
            }

            _statViews.Clear();
        }
    }
}
