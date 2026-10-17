using UnityEngine;

namespace Game.Logging
{
    public sealed class UnityConsoleLogSink : ILogSink
    {
        public void Log(string message) => Debug.Log(message);
    }
}
