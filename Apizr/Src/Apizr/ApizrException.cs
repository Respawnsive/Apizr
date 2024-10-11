using System;

namespace Apizr
{
    /// <summary>
    /// An exception with optional cached object
    /// </summary>
    public class ApizrException : Exception
    {
        public ApizrException(string message) : base(message)
        {
            
        }

        public ApizrException(Exception innerException) : base(innerException.Message, innerException)
        {

        }

        /// <summary>
        /// Indicates whether the exception has been handled yet by callback thanks to WithExCatching option.
        /// </summary>
        public bool Handled { get; internal set; }
    }

    /// <summary>
    /// An exception with optional cached <typeparamref name="TResult"/>
    /// </summary>
    public class ApizrException<TResult> : ApizrException
    {
        public ApizrException(string message) : base(message)
        {

        }

        public ApizrException(string message, TResult cachedResult) : base(message)
        {
            CachedResult = cachedResult;
        }

        public ApizrException(Exception innerException) : this(innerException, default)
        {

        }

        public ApizrException(Exception innerException, TResult cachedResult) : base(innerException)
        {
            CachedResult = cachedResult;
        }

        public TResult CachedResult { get; }
    }
}
