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
            ActionSystemStatus status = _system.Tick(context);

            if (context.Outcome != null && status != ActionSystemStatus.Running)
                _logSink.Log(context.Outcome.Format());

            return status;
        }

        public void Exit(ActionPhaseContext context) => _system.Exit(context);
    }
}
