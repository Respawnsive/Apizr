using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Extending;
using Apizr.Resiliencing;
using Microsoft.Extensions.Logging;

namespace Apizr.Authenticating
{
    /// <summary>
    /// The authentication handler base implementation
    /// </summary>
    public abstract class AuthenticationHandlerBase : DelegatingHandler, IAuthenticationHandler
    {
        protected readonly ILogger Logger;
        protected readonly IApizrManagerOptionsBase ApizrOptions;

        /// <summary>
        /// The authentication handler constructor
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="apizrOptions">The Apizr options</param>
        protected AuthenticationHandlerBase(ILogger logger, IApizrManagerOptionsBase apizrOptions)
        {
            Logger = logger;
            ApizrOptions = apizrOptions;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRequestMessage clonedRequest = null;
            string token = null;

            var context = request.GetOrBuildApizrResilienceContext(cancellationToken);
            if (!context.TryGetLogger(out var logger, out var logLevels, out _, out _))
            {
                logger = Logger;
                logLevels = ApizrOptions.LogLevels;
            }

            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                // Authorization required! Get the token from saved settings if available
                logger?.Log(logLevels.Low(), $"{context.OperationKey}: Authorization required with scheme {auth.Scheme}");
                token = await GetTokenAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(token))
                {
                    // We have one, then clone the request in case we need to re-issue it with a refreshed token
                    logger?.Log(logLevels.Low(), $"{context.OperationKey}: Saved token will be used");
                    clonedRequest = await this.CloneHttpRequestMessageAsync(request).ConfigureAwait(false);
                }
                else
                {
                    // Refresh the token
                    logger?.Log(logLevels.Low(), $"{context.OperationKey}: No token saved yet. Refreshing token...");
                    token = await this.RefreshTokenAsync(request).ConfigureAwait(false);
                }

                // Set the authentication header
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
                logger?.Log(logLevels.Low(), $"{context.OperationKey}: Authorization header has been set");
            }

            // Send the request
            logger?.Log(logLevels.Low(), $"{context.OperationKey}: Sending request...");
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            // Check if we get an Unauthorized response with token from settings
            if (response.StatusCode == HttpStatusCode.Unauthorized && auth != null && clonedRequest != null)
            {
                logger?.Log(logLevels.Medium(), $"{context.OperationKey}: Unauthorized !");

                // Refresh the token
                logger?.Log(logLevels.Low(), $"{context.OperationKey}: Refreshing token...");
                token = await this.RefreshTokenAsync(request).ConfigureAwait(false);

                // Set the authentication header with refreshed token 
                clonedRequest.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
                logger?.Log(logLevels.Low(), $"{context.OperationKey}: Authorization header has been set with refreshed token");

                // Send the request
                logger?.Log(logLevels.Low(), $"{context.OperationKey}: Sending request again but with refreshed authorization header...");
                response = await base.SendAsync(clonedRequest, cancellationToken).ConfigureAwait(false);
            }

            // Clear the token if unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = null;
                logger?.Log(logLevels.High(), $"{context.OperationKey}: Unauthorized ! Token has been cleared");
            }

            // Save the refreshed token if succeed or clear it if not
            await SetTokenAsync(token).ConfigureAwait(false);
            logger?.Log(logLevels.Low(), $"{context.OperationKey}: Token saved");

            return response;
        }

        /// <inheritdoc />
        public abstract Task<string> GetTokenAsync();

        /// <inheritdoc />
        public abstract Task SetTokenAsync(string token);

        /// <inheritdoc />
        public abstract Task<string> RefreshTokenAsync(HttpRequestMessage request);

        /// <summary>
        /// Clone a HttpRequestMessage
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>A copy of the request</returns>
        protected async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Version = request.Version,
#if NET6_0_OR_GREATER
                VersionPolicy = request.VersionPolicy
#endif
            };

            foreach (var prop in request.Properties)
                clone.Properties.Add(prop);

#if NET6_0_OR_GREATER
            foreach (var option in request.Options)
                clone.Options.Set(new HttpRequestOptionsKey<object>(option.Key), option.Value);
#endif

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            // Copy the request's content (via a MemoryStream) into the cloned object
            var memoryStream = new MemoryStream();
            if (request.Content != null)
            {
                await request.Content.CopyToAsync(memoryStream).ConfigureAwait(false);
                memoryStream.Position = 0;
                clone.Content = new StreamContent(memoryStream);

                // Copy the content headers
                foreach (var h in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            return clone;
        }
    }
}