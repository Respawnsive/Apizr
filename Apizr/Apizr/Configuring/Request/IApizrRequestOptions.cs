using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Apizr.Configuring.Request
{
    public interface IApizrRequestOptions : IApizrRequestOptionsBase
    {
        /// <summary>
        /// The Polly Context to pass through it all
        /// </summary>
        Context Context { get; }

        /// <summary>
        /// A cancellation token to pass through it all
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// Clear request cache before executing (default: false)
        /// </summary>
        bool ClearCache { get; }

        /// <summary>
        /// Handle exception and return cached result (default: null = throwing)
        /// </summary>
        Action<ApizrException> OnException { get; }

        /// <summary>
        /// 
        /// </summary>
        bool LetThrowOnExceptionWithEmptyCache { get; }

        /// <summary>
        /// Custom parameters to pass through delegating handlers
        /// </summary>
        IDictionary<string, object> HandlersParameters { get; }
    }
}
