using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr.Extending
{
    public static class HttpClientBuilderExtensions
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
                var httpClientFactory = s.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(builder.Name);

                return factory(httpClient, s);
            });

            return builder;
        }
    }
}
