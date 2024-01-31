using System;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Polly;

namespace Apizr.Resiliencing
{
    internal static class ResiliencePipelineExtensions
    {
        internal static ValueTask ExecuteAsync(this ResiliencePipeline policy,
            Func<IApizrRequestOptions, ValueTask> action, IApizrRequestOptionsBuilder requestOptionsBuilder) =>
            policy.ExecuteAsync((ctx, ct) =>
                {
                    requestOptionsBuilder.WithContext(ctx, ApizrDuplicateStrategy.Replace)
                        .WithCancellation(ct);

                    return action.Invoke(requestOptionsBuilder.ApizrOptions);
                },
                requestOptionsBuilder.ApizrOptions.Context,
                requestOptionsBuilder.ApizrOptions.CancellationToken);

        internal static ValueTask<TResult> ExecuteAsync<TResult>(this ResiliencePipeline<TResult> policy,
            Func<IApizrRequestOptions, ValueTask<TResult>> action, IApizrRequestOptionsBuilder requestOptionsBuilder) =>
            policy.ExecuteAsync((ctx, ct) =>
                {
                    requestOptionsBuilder.WithContext(ctx, ApizrDuplicateStrategy.Replace)
                        .WithCancellation(ct);

                    return action.Invoke(requestOptionsBuilder.ApizrOptions);
                },
                requestOptionsBuilder.ApizrOptions.Context,
                requestOptionsBuilder.ApizrOptions.CancellationToken);
    }
}
