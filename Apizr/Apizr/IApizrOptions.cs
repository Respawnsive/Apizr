using System;
using System.Net;
using System.Net.Http;
using HttpTracer;
using Refit;

namespace Apizr
{
    public interface IApizrOptions
    {
        Type WebApiType { get; }
        Uri BaseAddress { get; }
        DecompressionMethods DecompressionMethods { get; }
        HttpMessageParts HttpTracerVerbosity { get; }
        string[] PolicyRegistryKeys { get; }
        Func<RefitSettings> RefitSettingsFactory { get; }
        Func<DelegatingHandler> AuthenticationHandlerFactory { get; }
    }
}
