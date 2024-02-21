namespace Apizr.Configuring.Shared.Context;

public interface IApizrResilienceContextOptions
{
    /// <summary>
    /// Gets the value indicating whether to continue on captured context, if any.
    /// </summary>
    bool? ContinueOnCapturedContext { get; }

    /// <summary>
    /// Gets the value indicating whether to return the context to the pool, when request completes.
    /// </summary>
    bool? ReturnToPoolOnComplete { get; }
}