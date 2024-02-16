using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Configuring.Request;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr
{
    internal static class Constants
    {
        internal const string InterfaceTypeKey = "Refit.InterfaceType";
        internal const string ResilienceContextKey = "Resilience.Http.ResilienceContext";
        internal const string RequestMessageKey = "Resilience.Http.RequestMessage";
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
        internal static readonly ResiliencePropertyKey<HttpRequestMessage> RequestMessagePropertyKey = new(RequestMessageKey);
#if NET6_0_OR_GREATER
        internal static readonly HttpRequestOptionsKey<Type> InterfaceTypeOptionsKey = new(InterfaceTypeKey);
        internal static readonly HttpRequestOptionsKey<ResilienceContext> ResilienceContextOptionsKey = new(ResilienceContextKey);
        internal static readonly HttpRequestOptionsKey<IApizrRequestOptions> ApizrRequestOptionsOptionsKey = new(ApizrRequestOptionsKey);
#endif
    }
}
