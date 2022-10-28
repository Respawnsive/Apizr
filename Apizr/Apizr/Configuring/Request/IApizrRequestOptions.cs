using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Apizr.Configuring.Request
{
    public interface IApizrContextRequestOption : IApizrRequestOptionsBase
    {
        /// <summary>
        /// The Polly Context to pass through it all
        /// </summary>
        Context Context { get; }
    }

    public interface IApizrCancellationRequestOption : IApizrRequestOptionsBase
    {
        /// <summary>
        /// A cancellation token to pass through it all
        /// </summary>
        CancellationToken CancellationToken { get; }
    }

    public interface IApizrCacheRequestOption : IApizrRequestOptionsBase
    {
        /// <summary>
        /// Clear request cache before executing (default: false)
        /// </summary>
        bool ClearCache { get; }
    }

    public interface IApizrCatchRequestOption : IApizrRequestOptionsBase
    {
        /// <summary>
        /// Handle exception and return cached result (default: null = throwing)
        /// </summary>
        Action<ApizrException> OnException { get; }
    }

    /// <summary>
    /// Options available for a unit request
    /// </summary>
    public interface IApizrUnitRequestOptions : IApizrContextRequestOption, IApizrCancellationRequestOption
    {}

    /// <summary>
    /// Options available for a unit request with exception catching
    /// </summary>
    public interface IApizrCatchUnitRequestOptions : IApizrUnitRequestOptions, IApizrCatchRequestOption
    {}

    /// <summary>
    /// Options available for a result request
    /// </summary>
    public interface IApizrResultRequestOptions : IApizrUnitRequestOptions, IApizrCacheRequestOption
    {}

    /// <summary>
    /// Options available for a result request with exception catching
    /// </summary>
    public interface IApizrCatchResultRequestOptions : IApizrResultRequestOptions, IApizrCatchRequestOption
    {
        bool LetThrowOnExceptionWithEmptyCache { get; }
    }

    /// <summary>
    /// Options available for a request
    /// </summary>
    public interface IApizrRequestOptions : IApizrCatchUnitRequestOptions, IApizrCatchResultRequestOptions
    {}
}
