using System;
using System.Threading.Tasks;
using Optional;
using Optional.Async.Extensions;

namespace Apizr.Optional.Extending
{
    public static class OptionalMediationExtensions
    {
        public static Task<bool> OnResultAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Action<TResult> onResult)
        {
            var onResultFactory = new Func<TResult, Exception, Task<bool>>((result, exception) =>
            {
                onResult.Invoke(result);

                return Task.FromResult(true); // Never reached if there's an exception
            });

            return option.MatchAsync(some => OnSomeAsync(some, onResultFactory),
                none => OnNoneAsync(none, onResultFactory, true));
        }

        public static Task<bool> OnResultAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Func<TResult, bool> onResult)
            => OnResultAsync(option, (result, exception) => Task.FromResult(onResult.Invoke(result)));

        public static Task<bool> OnResultAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Func<TResult, Task<bool>> onResult)
            => OnResultAsync(option, (result, exception) => onResult.Invoke(result));

        public static Task<bool> OnResultAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Func<TResult, Exception, bool> onResult)
            => OnResultAsync(option, (result, exception) => Task.FromResult(onResult.Invoke(result, exception)));

        public static Task<bool> OnResultAsync<TResult>(
            this Task<Option<TResult, ApizrException<TResult>>> option, Func<TResult, Exception, Task<bool>> onResult)
        {
            return option.MatchAsync(some => OnSomeAsync(some, onResult),
                none => OnNoneAsync(none, onResult, false));
        }

        private static Task<bool> OnSomeAsync<TResult>(TResult result, Func<TResult, Exception, Task<bool>> onResult)
        {
            return onResult.Invoke(result, null);
        }

        private static Task<bool> OnNoneAsync<TResult>(ApizrException<TResult> exception,
            Func<TResult, Exception, Task<bool>> onResult, bool throwsOnException)
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
    }
}
