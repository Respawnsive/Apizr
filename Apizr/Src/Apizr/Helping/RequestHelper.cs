using System.Linq;
using System.Net.Http;

namespace Apizr.Helping
{
    internal static class RequestHelper
    {
        /// <summary>
        /// Cloned from Refit repository
        /// </summary>
        internal static void SetHeader(this HttpRequestMessage request, string name, string value)
        {
            // Clear any existing version of this header that might be set, because
            // we want to allow removal/redefinition of headers.
            // We also don't want to double up content headers which may have been
            // set for us automatically.

            // NB: We have to enumerate the header names to check existence because
            // Contains throws if it's the wrong header type for the collection.
            if (request.Headers.Any(x => x.Key == name))
            {
                request.Headers.Remove(name);
            }

            if (request.Content != null && request.Content.Headers.Any(x => x.Key == name))
            {
                request.Content.Headers.Remove(name);
            }

            if (value == null) return;

            var added = request.Headers.TryAddWithoutValidation(name, value);

            // Don't even bother trying to add the header as a content header
            // if we just added it to the other collection.
            if (!added && request.Content != null)
            {
                request.Content.Headers.TryAddWithoutValidation(name, value);
            }
        }
    }
}
