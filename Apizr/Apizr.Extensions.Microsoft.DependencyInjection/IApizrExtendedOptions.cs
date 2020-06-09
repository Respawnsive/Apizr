using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr
{
    public interface IApizrExtendedOptions : IApizrOptions
    {
        Type ApizrManagerType { get; }
        Type ConnectivityProviderType { get; }
        Type CacheProviderType { get; }
        Action<IHttpClientBuilder> HttpClientBuilder { get; }
        IList<Func<IServiceProvider, DelegatingHandler>> DelegatingHandlersExtendedFactories { get; }
    }
}
