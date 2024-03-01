using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Refit;

namespace Apizr
{
    /// <inheritdoc />
    public class ApizrResponse : IApizrResponse
    {
        private readonly IApiResponse _apiResponse;

        public ApizrResponse(IApiResponse apiResponse)
        {
            _apiResponse = apiResponse;
        }

        /// <inheritdoc />
        public HttpResponseHeaders Headers => _apiResponse.Headers;

        /// <inheritdoc />
        public HttpContentHeaders ContentHeaders => _apiResponse.ContentHeaders;

        /// <inheritdoc />
        public bool IsSuccessStatusCode => _apiResponse.IsSuccessStatusCode;

        /// <inheritdoc />
        public HttpStatusCode StatusCode => _apiResponse.StatusCode;

        /// <inheritdoc />
        public string ReasonPhrase => _apiResponse.ReasonPhrase;

        /// <inheritdoc />
        public HttpRequestMessage RequestMessage => _apiResponse.RequestMessage;

        /// <inheritdoc />
        public Version Version => _apiResponse.Version;

        /// <inheritdoc />
        public ApiException Error => _apiResponse.Error;
        
        #region Dispose

        private bool _disposed;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
                return;

            _disposed = true;

            _apiResponse.Dispose();
        }

        #endregion
    }

    /// <inheritdoc cref="IApizrResponse{T}" />
    public class ApizrResponse<T> : ApizrResponse, IApizrResponse<T>
    {
        /// <inheritdoc />
        public ApizrResponse(IApiResponse<T> apiResponse, T cachedContent = default) : base(apiResponse)
        {
            Content = apiResponse.Content ?? cachedContent;
        }

        /// <inheritdoc />
        public T Content { get; }
    }

    /// <summary>
    /// ApizrResponse extensions methods
    /// </summary>
    public static class ApizrResponseExtensions
    {
        /// <summary>
        /// Ensures the request was successful by throwing an exception in case of failure
        /// </summary>
        /// <returns>The current <see cref="IApizrResponse"/></returns>
        /// <exception cref="ApizrException"></exception>
        public static IApizrResponse EnsureSuccessStatusCode(this IApizrResponse apizrResponse)
        {
            if (!apizrResponse.IsSuccessStatusCode)
            {
                var exception = new ApizrException(apizrResponse.Error);

                apizrResponse.Dispose();

                throw exception;
            }

            return apizrResponse;
        }

        /// <summary>
        /// Ensures the request was successful by throwing an exception in case of failure
        /// </summary>
        /// <returns>The current <see cref="IApizrResponse{T}"/> with optional cached <typeparamref name="T"/> data</returns>
        /// <exception cref="ApizrException"></exception>
        public static IApizrResponse<T> EnsureSuccessStatusCode<T>(this IApizrResponse<T> apizrResponse,
            T cachedContent = default)
        {
            if (!apizrResponse.IsSuccessStatusCode)
            {
                var exception = new ApizrException(apizrResponse.Error, cachedContent);

                apizrResponse.Dispose();

                throw exception;
            }

            return apizrResponse;
        }
    }
}
