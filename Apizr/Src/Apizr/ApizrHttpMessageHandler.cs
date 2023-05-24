using Apizr.Policing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Helping;

namespace Apizr
{
    internal sealed class ApizrHttpMessageHandler : DelegatingHandler
    {
        static readonly ISet<HttpMethod> BodylessMethods = new HashSet<HttpMethod>
        {
            HttpMethod.Get,
            HttpMethod.Head
        };

        public ApizrHttpMessageHandler(HttpMessageHandler innerHandler):base(innerHandler)
        {
            
        }

        /// <inheritdoc />
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var options = request.GetApizrRequestOptions();
            if(options != null)
            {
                cancellationToken = options.CancellationToken;

                if (options.Headers.Length > 0)
                {
                    // Cloned and adjusted from Refit
                    // We could have content headers, so we need to make
                    // sure we have an HttpContent object to add them to,
                    // provided the HttpClient will allow it for the method
                    if (request.Content == null && !BodylessMethods.Contains(request.Method))
                        request.Content = new ByteArrayContent(Array.Empty<byte>());
                    
                    foreach (var header in options.Headers)
                    {
                        if (string.IsNullOrWhiteSpace(header)) continue;

                        // NB: Silverlight doesn't have an overload for String.Split()
                        // with a count parameter, but header values can contain
                        // ':' so we have to re-join all but the first part to get the
                        // value.
                        var parts = header.Split(':');
                        var headerKey = parts[0].Trim();
                        var headerValue = parts.Length > 1 ?
                            string.Join(":", parts.Skip(1)).Trim() : null;

                        request.SetHeader(headerKey, headerValue);
                    }
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
