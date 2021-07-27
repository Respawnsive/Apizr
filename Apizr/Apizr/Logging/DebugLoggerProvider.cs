using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    internal class DebugLoggerProvider : ILoggerProvider
    {
        private readonly LogLevel _logLevel;

        public DebugLoggerProvider(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        public ILogger CreateLogger(string categoryName) => new DebugLogger(categoryName, _logLevel);

        public void Dispose()
        {
        }
    }
}
