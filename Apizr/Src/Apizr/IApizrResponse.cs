using System;
using System.Diagnostics.CodeAnalysis;
using Refit;

namespace Apizr
{
    /// <summary>
    /// Base interface used to represent an API response managed by Apizr.
    /// </summary>
    public interface IApizrResponse : IDisposable
    {
        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        bool IsSuccess { get; }

        /// <summary>
        /// Base interface used to represent an API response.
        /// </summary>
        IApiResponse ApiResponse { get; }

        /// <summary>
        /// The <see cref="ApizrException"/> object in case of unsuccessful response.
        /// </summary>
        ApizrException Exception { get; }

        /// <summary>
        /// Indicates whether the exception has been handled yet by callback thanks to WithExCatching option.
        /// </summary>
        bool IsExceptionHandled { get; }
    }

    /// <summary>
    /// Interface used to represent an API response managed by Apizr.
    /// </summary>
    /// <typeparam name="TResult">Deserialized request content as <typeparamref name="TResult"/></typeparam>
    public interface IApizrResponse<out TResult> : IApizrResponse
    {
        /// <summary>
        /// Deserialized request or cache content as <typeparamref name="TResult"/>.
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// The source of the result data (might be from the request or the cache).
        /// </summary>
        ApizrResponseDataSource DataSource { get; }
    }

    public enum ApizrResponseDataSource
    {
        None,
        Request,
        Cache
    }
}
