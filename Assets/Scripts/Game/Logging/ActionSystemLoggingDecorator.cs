using System;
using Action = Actors.Actions.Action;
using Game.Phases.Action;
using Game.Phases.Action.Systems;

namespace Game.Logging
{
    public sealed class ActionSystemLoggingDecorator : IActionSystem
    {
        private readonly IActionSystem _system;
        private readonly ILogSink _logSink;

        public ActionSystemLoggingDecorator(IActionSystem system, ILogSink logSink)
        {
            _system = system;
            _logSink = logSink;
        }

        public void Enter(ActionPhaseContext context) => _system.Enter(context);

        public ActionSystemStatus Tick(ActionPhaseContext context)
        {
            Action action = context.SelectedAction;
            ActionSystemStatus status = _system.Tick(context);

            if (action != null && status != ActionSystemStatus.Running)
            {
                string actorName = context.Turn.Actor.Definition.GivenName;
                _logSink.Log(
                    $"[Action] Round: {context.Turn.Round}, " +
                    $"Actor: {actorName} UniqueId: {context.Turn.Actor.UniqueId}, " +
                    $"Action: {action.Definition.ActionName}.");
            }

            return status;
        }

        public void Exit(ActionPhaseContext context) => _system.Exit(context);
    }
}
