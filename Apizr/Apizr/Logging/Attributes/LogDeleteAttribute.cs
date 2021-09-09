using System;
using HttpTracer;
using Microsoft.Extensions.Logging;

namespace Apizr.Logging.Attributes
{
    /// <summary>
    /// Tells Apizr to trace and log HTTP(s) traffic
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class LogDeleteAttribute : LogAttributeBase
    {
        public LogDeleteAttribute()
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity) : base(trafficVerbosity)
        {
        }

        public LogDeleteAttribute(LogLevel logLevel) : base(logLevel)
        {
        }

        public LogDeleteAttribute(HttpMessageParts trafficVerbosity, LogLevel logLevel) : base(trafficVerbosity, logLevel)
        {
        }
    }
}
