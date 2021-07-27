using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    public class HttpTracerLogWrapper : HttpTracer.Logger.ILogger
    {
        private readonly ILogger _logger;
        private readonly IApizrOptionsBase _apizrOptions;

        public HttpTracerLogWrapper(ILogger logger, IApizrOptionsBase apizrOptions)
        {
            _logger = logger;
            _apizrOptions = apizrOptions;
        }

        public void Log(string message) => _logger.Log(_apizrOptions.TrafficLogLevel, message);
    }
}
