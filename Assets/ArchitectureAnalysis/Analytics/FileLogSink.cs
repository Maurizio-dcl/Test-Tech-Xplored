using System;
using System.IO;
using Game.Logging;

namespace ArchitectureAnalysis.Analytics
{
    // Example local destination for ILogSink messages
    public sealed class FileLogSink : ILogSink
    {
        private readonly string _filePath;

        public FileLogSink(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            // ...write to file
        }
    }
}