using System;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultLogger : ILogger
    {
        readonly string _categoryName;
        readonly LogLevel _configLogLevel;


        public DefaultLogger(string categoryName, LogLevel logLevel)
        {
            _categoryName = categoryName;
            _configLogLevel = logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            // TODO: I need scopes
            var message = $"[{logLevel} - {_categoryName}] {formatter(state, exception)}";

            Console.WriteLine(message);
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _configLogLevel;

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
    }
}
