using HttpTracer.Logger;

namespace Apizr.Logging
{
    public class HttpTracerLogWrapper : ILogger
    {
        private readonly ILogHandler _logHandler;

        public HttpTracerLogWrapper(ILogHandler logHandler)
        {
            _logHandler = logHandler;
        }

        public void Log(string message)
        {
            _logHandler.Write(message);
        }
    }
}
