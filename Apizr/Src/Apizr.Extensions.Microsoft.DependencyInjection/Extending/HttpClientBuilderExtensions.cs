using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace Apizr.Extending
{
    internal static class HttpClientBuilderExtensions
    {
        internal static IHttpClientBuilder AddTypedClient(this IHttpClientBuilder builder, Type type, Func<HttpClient, IServiceProvider, object> factory)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            builder.Services.AddTransient(type, s =>
            {
                var httpMessageHandlerFactory = s.GetRequiredService<IHttpMessageHandlerFactory>();

                var handler = httpMessageHandlerFactory.CreateHandler(builder.Name);
                var httpClient = new ApizrHttpClient(handler, disposeHandler: false);

                var httpClientFactoryOptions = s.GetRequiredService<IOptionsMonitor<HttpClientFactoryOptions>>();

                var options = httpClientFactoryOptions.Get(builder.Name);
                foreach (var httpClientActions in options.HttpClientActions)
                {
                    httpClientActions(httpClient);
                }

                return factory(httpClient, s);
            });

            return builder;
        }
    }
}
