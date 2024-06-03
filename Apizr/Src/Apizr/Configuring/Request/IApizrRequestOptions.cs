using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Apizr.Configuring.Shared;
using Apizr.Configuring.Shared.Context;

namespace Apizr.Configuring.Request
{
    /// <inheritdoc cref="IApizrRequestOptionsBase" />
    public interface IApizrRequestOptions : IApizrRequestOptionsBase, IApizrGlobalSharedOptions
    {
        /// <summary>
        /// A cancellation token to pass through it all
        /// </summary>
        CancellationToken CancellationToken { get; }

        /// <summary>
        /// Clear request cache before executing (default: false)
        /// </summary>
        bool ClearCache { get; }

        /// <summary>
        /// Options set to resilience context
        /// </summary>
        IApizrResilienceContextOptions ResilienceContextOptions { get; }

        /// <summary>
        /// Headers to add to the request
        /// </summary>
        IList<string> Headers { get; }

        internal IList<string> HeadersStore { get; }

        internal Expression OriginalExpression { get; set; }

        internal string[] RequestResiliencePipelineKeys { get; set; }
    }
}
