using System;
using Apizr.Extending;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultLogger : ILogger
    {
        protected readonly string CategoryName;
        protected readonly LogLevel ConfigLogLevel;


        public DefaultLogger(string categoryName, LogLevel logLevel)
        {
            CategoryName = categoryName;
            ConfigLogLevel = logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            // TODO: I need scopes
            var message = $"[{logLevel} - {CategoryName}] {formatter(state, exception)}";

            Console.WriteLine(message);
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= ConfigLogLevel;

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
    }
}
