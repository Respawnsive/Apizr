using System;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Extending
{
    public static class OptionalMediationExtensions
    {
        /// <summary>
        /// The action will be invoked just before throwing any exception that might have occurred during request execution
        /// </summary>
        /// <typeparam name="TResult">The returned result (from fetch if succeed or cache if failed)</typeparam>
        /// <param name="option"></param>
        /// <param name="onResult">The action to invoke</param>
        /// <returns></returns>
        public static Task OnResultAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Action<TResult> onResult)
        {
            var onResultFactory = new Func<TResult, ApizrException<TResult>, Task<bool>>((result, exception) =>
            {
                onResult.Invoke(result);

                return Task.FromResult(true); // Never reached if there's an exception
            });

            return option.MatchAsync(some => OnSomeAsync(some, onResultFactory),
                none => OnNoneAsync(none, onResultFactory, true));
        }

        /// <summary>
        /// The function will be invoked with the returned result and potential occurred exception.
        /// You might decide to throw from the function itself, or to return the success boolean.
        /// </summary>
        /// <typeparam name="TResult">The returned result (from fetch if succeed or cache if failed)</typeparam>
        /// <param name="option"></param>
        /// <param name="onResult">The function to invoke</param>
        /// <returns></returns>
        public static Task<bool> OnResultAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Func<TResult, ApizrException<TResult>, bool> onResult)
            => OnResultAsync(option, (result, exception) => Task.FromResult(onResult.Invoke(result, exception)));

        /// <summary>
        /// The function will be invoked with the returned result and potential occurred exception.
        /// Checking exception, you might decide to throw it from the function itself, or to return the success boolean.
        /// </summary>
        /// <typeparam name="TResult">The returned result (from fetch if succeed or cache if failed)</typeparam>
        /// <param name="option"></param>
        /// <param name="onResult">The function to invoke</param>
        public static Task<bool> OnResultAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Func<TResult, ApizrException<TResult>, Task<bool>> onResult)
        {
            return option.MatchAsync(some => OnSomeAsync(some, onResult),
                none => OnNoneAsync(none, onResult, false));
        }

        private static Task<bool> OnSomeAsync<TResult>(TResult result, Func<TResult, ApizrException<TResult>, Task<bool>> onResult)
        {
            return onResult.Invoke(result, null);
        }

        private static Task<bool> OnNoneAsync<TResult>(ApizrException<TResult> exception,
            Func<TResult, ApizrException<TResult>, Task<bool>> onResult, bool throwsOnException)
        {
            try
            {
                return onResult.Invoke(exception.CachedResult, exception);
            }
            finally
            {
                if (throwsOnException)
                    throw exception;
            }
        }

        /// <summary>
        /// Return <see cref="TResult"/> (from fetch or cache), no matter of exception (handled by <see cref="onException"/>).
        /// Could throw if you ask to with <see cref="letThrowOnExceptionWithEmptyCache"/> in case of exception with empty cache (<see cref="onException"/> won't be called),
        /// otherwise return the empty cache (have to be managed) after calling <see cref="onException"/> action.
        /// </summary>
        /// <typeparam name="TResult">The returned result (from fetch if succeed or cache if failed)</typeparam>
        /// <param name="option"></param>
        /// <param name="onException">Action to call to handle exception (like informing the user) before returning result from cache</param>
        /// <param name="letThrowOnExceptionWithEmptyCache">True to let it throw the inner exception in case of empty cache, False to handle it with <see cref="onException"/> action and return empty cache result</param>
        /// <returns></returns>
        public static Task<TResult> CatchAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Action<Exception> onException, bool letThrowOnExceptionWithEmptyCache = false)
        {
            return option.MatchAsync(some => Task.FromResult(some), none =>
            {
                if (letThrowOnExceptionWithEmptyCache && none.CachedResult == null)
                    throw none;

                onException(none);
                return Task.FromResult(none.CachedResult);
            });
        }
    }
}
