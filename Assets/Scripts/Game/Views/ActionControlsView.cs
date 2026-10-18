using System;
using System.Collections.Generic;
using Actors;
using TMPro;
using UnityEngine;
using Action = Actors.Actions.Action;

namespace Game.Views
{
    public sealed class ActionControlsView : MonoBehaviour
    {
        [SerializeField] private ActionView actionViewPrefab;
        [SerializeField] private Transform actionsContainer;
        [SerializeField] private TMP_Text actionName;
        [SerializeField] private TMP_Text actionDescription;

        private readonly List<ActionView> _actionViews = new();
        private Action _displayedAction;
        private Action _selectedAction;

        public event Action<Action> ActionRequested;

        private void Awake() => actionsContainer.GetComponentsInChildren(true, _actionViews);

        private void OnDestroy()
        {
            for (int i = 0; i < _actionViews.Count; i++)
                Unsubscribe(_actionViews[i]);
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
                Subscribe(actionView);
                _actionViews.Add(actionView);
            }

            ResetActionDescription();
        }

        public void Refresh()
        {
            for (int i = 0; i < _actionViews.Count; i++)
                _actionViews[i].Refresh();
        }

        public void ResetActionDescription()
        {
            _displayedAction = null;
            _selectedAction = null;
            actionName.text = string.Empty;
            actionDescription.text = string.Empty;
        }

        private void ClearActionViews()
        {
            for (int i = 0; i < _actionViews.Count; i++)
            {
                ActionView actionView = _actionViews[i];
                Unsubscribe(actionView);
                actionView.gameObject.SetActive(false);
                Destroy(actionView.gameObject);
            }

            _actionViews.Clear();
        }

        private void Subscribe(ActionView actionView)
        {
            actionView.ActionRequested += OnActionRequested;
            actionView.ActionHovered += OnActionHovered;
            actionView.ActionUnhovered += OnActionUnhovered;
        }

        private void Unsubscribe(ActionView actionView)
        {
            actionView.ActionRequested -= OnActionRequested;
            actionView.ActionHovered -= OnActionHovered;
            actionView.ActionUnhovered -= OnActionUnhovered;
        }

        private void OnActionRequested(Action action)
        {
            if (_selectedAction == null)
            {
                _selectedAction = action;
                ShowActionDescription(action);
            }

            ActionRequested?.Invoke(action);
        }

        private void OnActionHovered(Action action)
        {
            if (_selectedAction == null)
                ShowActionDescription(action);
        }

        private void OnActionUnhovered(Action action)
        {
            if (_selectedAction == null && _displayedAction == action)
                ResetActionDescription();
        }

        private void ShowActionDescription(Action action)
        {
            _displayedAction = action;
            actionName.text = action.Definition.ActionName;
            actionDescription.text = action.Definition.ActionDescription;
        }
    }
}
