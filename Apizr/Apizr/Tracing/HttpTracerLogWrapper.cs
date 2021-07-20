using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Apizr.Tracing
{
    public class HttpTracerLogWrapper : HttpTracer.Logger.ILogger
    {
        private readonly ILogger _logger;
        private readonly IApizrOptionsBase _apizrOptions;
        private bool _hasBeenCheckedOnce;

        public HttpTracerLogWrapper(ILogger logger, IApizrOptionsBase apizrOptions)
        {
            _logger = logger;
            _apizrOptions = apizrOptions;
        }

        public void Log(string message)
        {
            if (!_hasBeenCheckedOnce)
            {
                if(_logger is NullLogger)
                    Console.WriteLine("You asked to trace http traffic but did not provide any logger. Please provide one by calling WithLogging fluent registration option method or configuring it with ILoggingBuilder.");

                _hasBeenCheckedOnce = true;
            }

            _logger.Log(_apizrOptions.TrafficLogLevel, message);
        } 
    }
}
