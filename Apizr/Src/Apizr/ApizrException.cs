using System;

namespace Apizr
{
    /// <summary>
    /// An exception with optional cached object
    /// </summary>
    public class ApizrException : Exception
    {
        public ApizrException(Exception innerException) : this(innerException, default)
        {
            
        }

        public ApizrException(Exception innerException, object cachedResult) : base(innerException.Message, innerException)
        {
            CachedResult = cachedResult;
        }

        public object CachedResult { get; }
    }

    /// <summary>
    /// An exception with optional cached <typeparamref name="TResult"/>
    /// </summary>
    public class ApizrException<TResult> : ApizrException
    {
        public ApizrException(Exception innerException) : this(innerException, default)
        {

        }

        public ApizrException(Exception innerException, TResult cachedResult) : base(innerException, cachedResult)
        {
            CachedResult = cachedResult;
        }

        public new TResult CachedResult { get; }
    }
}
