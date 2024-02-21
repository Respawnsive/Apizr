namespace Apizr.Configuring.Shared.Context;

public interface IApizrResilienceContextOptionsBuilder
{
    internal IApizrResilienceContextOptions ResilienceContextOptions { get; }

    IApizrResilienceContextOptionsBuilder ContinueOnCapturedContext(bool continueOnCapturedContext);

    IApizrResilienceContextOptionsBuilder ReturnToPoolOnComplete(bool returnToPoolOnComplete);
}