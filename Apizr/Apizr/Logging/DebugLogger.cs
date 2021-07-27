using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    internal sealed class DebugLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly LogLevel _logLevel;
        
        public DebugLogger(string categoryName, LogLevel logLevel)
        {
            _categoryName = categoryName;
            _logLevel = logLevel;
        }

        public bool IsEnabled(LogLevel logLevel) => Debugger.IsAttached && logLevel == _logLevel;

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"{ logLevel }: {message}";

            if (exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception;
            }

            Debug.WriteLine(message, _categoryName);
        }
    }
}
