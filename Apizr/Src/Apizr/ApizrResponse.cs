using Refit;

namespace Apizr
{
    /// <inheritdoc />
    public class ApizrResponse : IApizrResponse
    {
        protected ApizrResponse()
        {
            
        }

        public ApizrResponse(IApiResponse apiResponse)
        {
            ApiResponse = apiResponse;
            if(apiResponse.Error != null)
                Exception = new ApizrException(apiResponse.Error);
        }

        public ApizrResponse(ApizrException apiException)
        {
            Exception = apiException;
        }

        public ApizrResponse(IApiResponse apiResponse, ApizrException apizrException)
        {
            ApiResponse = apiResponse;
            Exception = apizrException;
        }

        /// <inheritdoc />
        public bool IsSuccess => Exception == null;

        /// <inheritdoc />
        public IApiResponse ApiResponse { get; }

        /// <inheritdoc />
        public ApizrException Exception { get; }

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

            ApiResponse.Dispose();
        }

        #endregion
    }

    /// <inheritdoc cref="IApizrResponse{TResult}" />
    public class ApizrResponse<TResult> : ApizrResponse, IApizrResponse<TResult>
    {
        public ApizrResponse(TResult cachedResult) : base()
        {
            Result = cachedResult;
            DataSource = ApizrResponseDataSource.Cache;
        }

        /// <inheritdoc />
        public ApizrResponse(IApiResponse<TResult> apiResponse) : base(apiResponse)
        {
            if (apiResponse.Content != null)
            {
                Result = apiResponse.Content;
                DataSource = ApizrResponseDataSource.Request;
            }
        }

        public ApizrResponse(ApizrException<TResult> apizrException) : base(apizrException)
        {
            Result = apizrException.CachedResult;
            DataSource = ApizrResponseDataSource.Cache;
        }

        /// <inheritdoc />
        public ApizrResponse(IApiResponse<TResult> apiResponse, ApizrException<TResult> apizrException) : base(apiResponse, apizrException)
        {
            if (apiResponse.Content != null)
            {
                Result = apiResponse.Content;
                DataSource = ApizrResponseDataSource.Request;
            }
            else if (apizrException.CachedResult != null)
            {
                Result = apizrException.CachedResult;
                DataSource = ApizrResponseDataSource.Cache;
            }
        }

        /// <inheritdoc />
        public TResult Result { get; }

        /// <inheritdoc />
        public ApizrResponseDataSource DataSource { get; }
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
            if (!apizrResponse.IsSuccess)
            {
                var exception = apizrResponse.Exception ?? new ApizrException("Unknown exception occured");

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
        public static IApizrResponse<T> EnsureSuccessStatusCode<T>(this IApizrResponse<T> apizrResponse)
        {
            if (!apizrResponse.IsSuccess)
            {
                var exception = apizrResponse.Exception ?? new ApizrException<T>("Unknown exception occured", apizrResponse.Result);

                apizrResponse.Dispose();

                throw exception;
            }

            return apizrResponse;
        }
    }
}
