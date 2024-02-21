using System.Linq.Expressions;
using System.Threading;
using Apizr.Configuring.Shared.Context;

namespace Apizr.Configuring.Request
{
    /// <inheritdoc />
    public interface IApizrRequestOptions : IApizrRequestOptionsBase
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

        internal Expression OriginalExpression { get; set; }
    }
}
