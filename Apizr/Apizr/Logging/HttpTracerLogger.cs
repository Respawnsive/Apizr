using HttpTracer.Logger;

namespace Apizr.Logging
{
    public class HttpTracerLogger : ILogger
    {
        private readonly ILogHandler _logHandler;

        public HttpTracerLogger(ILogHandler logHandler)
        {
            _logHandler = logHandler;
        }

        public void Log(string message)
        {
            _logHandler.Write(message);
        }
    }
}
