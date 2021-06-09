using Microsoft.Extensions.Logging;

namespace Apizr.Logging
{
    public class HttpTracerLogWrapper : HttpTracer.Logger.ILogger
    {
        private readonly ILogger _logger;

        public HttpTracerLogWrapper(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string message) => _logger.LogTrace(message);
    }
}
