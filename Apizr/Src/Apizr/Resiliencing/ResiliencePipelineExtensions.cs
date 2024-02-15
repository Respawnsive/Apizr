using System;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Polly;

namespace Apizr.Resiliencing
{
    internal static class ResiliencePipelineExtensions
    {
        internal static ValueTask ExecuteAsync(this ResiliencePipeline pipeline,
            Func<IApizrRequestOptions, ValueTask> action, IApizrRequestOptionsBuilder requestOptionsBuilder) =>
            pipeline.ExecuteAsync(ctx =>
                {
                    requestOptionsBuilder.WithContext(ctx, ApizrDuplicateStrategy.Replace)
                        .WithCancellation(ctx.CancellationToken);

                    return action.Invoke(requestOptionsBuilder.ApizrOptions);
                },
                requestOptionsBuilder.ApizrOptions.ResilienceContext);

        internal static ValueTask<TResult> ExecuteAsync<TResult>(this ResiliencePipeline<TResult> pipeline,
            Func<IApizrRequestOptions, ValueTask<TResult>> action, IApizrRequestOptionsBuilder requestOptionsBuilder) =>
            pipeline.ExecuteAsync((ctx, ct) =>
                {
                    requestOptionsBuilder.WithContext(ctx, ApizrDuplicateStrategy.Replace)
                        .WithCancellation(ct);

                    return action.Invoke(requestOptionsBuilder.ApizrOptions);
                },
                requestOptionsBuilder.ApizrOptions.Context,
                requestOptionsBuilder.ApizrOptions.CancellationToken);
    }
}
