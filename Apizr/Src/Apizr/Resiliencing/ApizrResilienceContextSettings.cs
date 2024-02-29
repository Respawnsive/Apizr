using Polly;

namespace Apizr.Resiliencing
{
#pragma warning disable CA1815 // Override equals and operator equals on value types

    /// <summary>
    /// Settings used by Apizr when dealing with <see cref="ResilienceContext"/>.
    /// </summary>
    public readonly struct ApizrResilienceContextSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApizrResilienceContextSettings"/> struct.
        /// </summary>
        /// <param name="continueOnCapturedContext">Value indicating whether to continue on captured context.</param>
        /// <param name="returnToPoolOnComplete"></param>
        public ApizrResilienceContextSettings(bool? continueOnCapturedContext = null, bool? returnToPoolOnComplete = null)
        {
            ContinueOnCapturedContext = continueOnCapturedContext;
            ReturnToPoolOnComplete = returnToPoolOnComplete;
        }

        /// <summary>
        /// Gets the value indicating whether to continue on captured context, if any.
        /// </summary>
        public bool? ContinueOnCapturedContext { get; }

        /// <summary>
        /// Gets the value indicating whether to return the context to the pool, when request completes.
        /// </summary>
        public bool? ReturnToPoolOnComplete { get; }
    }
}
