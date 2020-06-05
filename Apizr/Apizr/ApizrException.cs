using System;

namespace Apizr
{

    public class ApizrException : Exception
    {
        public ApizrException(Exception innerException, object cachedResult) : base(innerException.Message, innerException)
        {
            CachedResult = cachedResult;
        }

        public object CachedResult { get; }
    }

    public class ApizrException<TResult> : ApizrException
    {
        public ApizrException(Exception innerException, TResult cachedResult) : base(innerException, cachedResult)
        {
            CachedResult = cachedResult;
        }

        public new TResult CachedResult { get; }
    }
}
