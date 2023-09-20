using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace Apizr
{
    internal static class Constants
    {
        internal const string InterfaceTypeKey = "Refit.InterfaceType";
        internal const string PollyExecutionContextKey = "PollyExecutionContext";
        internal const string PriorityKey = "Priority";
        internal const string ApizrRequestOptionsKey = "ApizrRequestOptions";
        internal const string ApizrProgressKey = "ApizrProgressKey";
        internal const string ApizrDynamicPathKey = "ApizrDynamicPathKey";
        internal const string ApizrIgnoreMessagePartsKey = "ApizrIgnoreMessagePartsKey";
        internal const string ApizrOptionsProcessedKey = "ApizrOptionsProcessedKey";
        internal const LogLevel LowLogLevel = LogLevel.Trace;
        internal const LogLevel MediumLogLevel = LogLevel.Information;
        internal const LogLevel HighLogLevel = LogLevel.Critical;
        internal static readonly ISet<HttpMethod> BodylessMethods = new HashSet<HttpMethod> { HttpMethod.Get, HttpMethod.Head };
    }
}
