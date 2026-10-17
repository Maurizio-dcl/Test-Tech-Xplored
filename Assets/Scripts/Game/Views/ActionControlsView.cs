using System;
using System.Collections.Generic;
using Actors;
using UnityEngine;
using Action = Actors.Actions.Action;

namespace Game.Views
{
    public sealed class ActionControlsView : MonoBehaviour
    {
        [SerializeField] private ActionView actionViewPrefab;
        [SerializeField] private Transform actionsContainer;

        private readonly List<ActionView> _actionViews = new();

        public event Action<Action> ActionRequested;

        private void Awake() => actionsContainer.GetComponentsInChildren(true, _actionViews);

        private void OnDestroy()
        {
            for (int i = 0; i < _actionViews.Count; i++)
                _actionViews[i].ActionRequested -= OnActionRequested;
        }

        public void Setup(Actor actor)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            ClearActionViews();

            IReadOnlyList<Action> actions = actor.Actions;
            for (int i = 0; i < actions.Count; i++)
            {
                ActionView actionView = Instantiate(actionViewPrefab, actionsContainer);
                actionView.Setup(actions[i]);
                actionView.ActionRequested += OnActionRequested;
                _actionViews.Add(actionView);
            }
        }

        private void ClearActionViews()
        {
            for (int i = 0; i < _actionViews.Count; i++)
            {
                ActionView actionView = _actionViews[i];
                actionView.ActionRequested -= OnActionRequested;
                actionView.gameObject.SetActive(false);
                Destroy(actionView.gameObject);
            }

            _actionViews.Clear();
        }

        private void OnActionRequested(Action action) => ActionRequested?.Invoke(action);
    }
}
