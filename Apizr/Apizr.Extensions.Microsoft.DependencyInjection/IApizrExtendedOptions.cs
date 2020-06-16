using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr
{
    public interface IApizrExtendedOptions : IApizrOptions
    {
        Type ApizrManagerType { get; }
        Type ConnectivityHandlerType { get; }
        Type CacheHandlerType { get; }
        Type LogHandlerType { get; }
        Action<IHttpClientBuilder> HttpClientBuilder { get; }
        IList<Func<IServiceProvider, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
    }
}
