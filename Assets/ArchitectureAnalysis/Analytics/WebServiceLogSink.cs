using System;
using System.Text;
using Game.Logging;
using UnityEngine.Networking;

namespace ArchitectureAnalysis.Analytics
{
    // Example remote destination. Authentication and retry policies would be added by the real service.
    public sealed class WebServiceLogSink : ILogSink
    {
        private readonly string _endpoint;

        public WebServiceLogSink(string endpoint)
        {
            _endpoint = endpoint;
        }

        public void Log(string message)
        {
            //...send web request
        }
    }
}
