using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Logging;

namespace Apizr.Authenticating
{
    public abstract class AuthenticationHandlerBase : DelegatingHandler, IAuthenticationHandler
    {
        private readonly ILogHandler _logHandler;

        protected AuthenticationHandlerBase(ILogHandler logHandler)
        {
            _logHandler = logHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpRequestMessage clonedRequest = null;
            string token = null;

            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                // Authorization required! Get the token from saved settings if available
                _logHandler.Write($"Apizr - {GetType().Name}: Authorization required with scheme {auth.Scheme}");
                token = this.GetToken();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    // We have one, then clone the request in case we need to re-issue it with a refreshed token
                    _logHandler.Write($"Apizr - {GetType().Name}: Saved token will be used");
                    clonedRequest = await this.CloneHttpRequestMessageAsync(request);
                }
                else
                {
                    // Refresh the token
                    _logHandler.Write($"Apizr - {GetType().Name}: No token saved yet. Refreshing token...");
                    token = await this.RefreshTokenAsync(request).ConfigureAwait(false);
                }

                // Set the authentication header
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
                _logHandler.Write($"Apizr - {GetType().Name}: Authorization header has been set");
            }

            // Send the request
            _logHandler.Write($"Apizr - {GetType().Name}: Sending request with authorization header...");
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            // Check if we get an Unauthorized response with token from settings
            if (response.StatusCode == HttpStatusCode.Unauthorized && auth != null && clonedRequest != null)
            {
                _logHandler.Write($"Apizr - {GetType().Name}: Unauthorized !");

                // Refresh the token
                _logHandler.Write($"Apizr - {GetType().Name}: Refreshing token...");
                token = await this.RefreshTokenAsync(request).ConfigureAwait(false);

                // Set the authentication header with refreshed token 
                clonedRequest.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
                _logHandler.Write($"Apizr - {GetType().Name}: Authorization header has been set with refreshed token");

                // Send the request
                _logHandler.Write($"Apizr - {GetType().Name}: Sending request again but with refreshed authorization header...");
                response = await base.SendAsync(clonedRequest, cancellationToken).ConfigureAwait(false);
            }

            // Clear the token if unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = null;
                _logHandler.Write($"Apizr - {GetType().Name}: Unauthorized ! Token has been cleared");
            }

            // Save the refreshed token if succeed or clear it if not
            this.SetToken(token);
            _logHandler.Write($"Apizr - {GetType().Name}: Token saved");

            return response;
        }

        protected abstract string GetToken();

        protected abstract void SetToken(string token);

        protected abstract Task<string> RefreshTokenAsync(HttpRequestMessage request);

        /// <summary>
        /// Clone a HttpRequestMessage
        /// Credit: http://stackoverflow.com/questions/25044166/how-to-clone-a-httprequestmessage-when-the-original-request-has-content
        /// </summary>
        /// <param name="req">The request</param>
        /// <returns>A copy of the request</returns>
        protected async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req)
        {
            var clone = new HttpRequestMessage(req.Method, req.RequestUri);

            // Copy the request's content (via a MemoryStream) into the cloned object
            var ms = new MemoryStream();
            if (req.Content != null)
            {
                await req.Content.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                clone.Content = new StreamContent(ms);

                // Copy the content headers
                if (req.Content.Headers != null)
                    foreach (var h in req.Content.Headers)
                        clone.Content.Headers.Add(h.Key, h.Value);
            }


            clone.Version = req.Version;

            foreach (var prop in req.Properties)
                clone.Properties.Add(prop);

            foreach (var header in req.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            return clone;
        }
    }
}