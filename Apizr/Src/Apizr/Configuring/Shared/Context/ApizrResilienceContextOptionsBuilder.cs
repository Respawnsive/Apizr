namespace Apizr.Configuring.Shared.Context;

public class ApizrResilienceContextOptionsBuilder : IApizrResilienceContextOptionsBuilder
{
    /// <summary>
    /// The common options
    /// </summary>
    protected readonly ApizrResilienceContextOptions Options;

    internal ApizrResilienceContextOptionsBuilder(ApizrResilienceContextOptions commonOptions)
    {
        Options = commonOptions;
    }

    /// <inheritdoc />
    IApizrResilienceContextOptions IApizrResilienceContextOptionsBuilder.ResilienceContextOptions => Options;

    /// <inheritdoc />
    public IApizrResilienceContextOptionsBuilder ContinueOnCapturedContext(bool continueOnCapturedContext)
    {
        Options.ContinueOnCapturedContext = continueOnCapturedContext;

        return this;
    }

    /// <inheritdoc />
    public IApizrResilienceContextOptionsBuilder ReturnToPoolOnComplete(bool returnToPoolOnComplete)
    {
        Options.ReturnToPoolOnComplete = returnToPoolOnComplete;

        return this;
    }
}