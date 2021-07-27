using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    internal class DebugLoggerFactory : ILoggerFactory
    {
        private readonly LogLevel _logLevel;

        public DebugLoggerFactory(LogLevel logLevel)
        {
            _logLevel = logLevel;
        }

        public ILogger CreateLogger(string categoryName) => new DebugLogger(categoryName, _logLevel);

        public void AddProvider(ILoggerProvider provider)
        {

        }

        public void Dispose()
        {

        }
    }
}
