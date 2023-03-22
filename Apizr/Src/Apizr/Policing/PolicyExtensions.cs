using System;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Polly;

namespace Apizr.Policing
{
    internal static class PolicyExtensions
    {
        internal static Task ExecuteAsync(this IAsyncPolicy policy,
            Func<IApizrRequestOptions, Task> action, IApizrRequestOptionsBuilder requestOptionsBuilder) =>
            policy.ExecuteAsync((ctx, ct) =>
                {
                    requestOptionsBuilder.WithContext(ctx, ApizrDuplicateStrategy.Replace)
                        .WithCancellation(ct);

                    return action.Invoke(requestOptionsBuilder.ApizrOptions);
                },
                requestOptionsBuilder.ApizrOptions.Context, requestOptionsBuilder.ApizrOptions.CancellationToken);

        internal static Task<TResult> ExecuteAsync<TResult>(this IAsyncPolicy<TResult> policy,
            Func<IApizrRequestOptions, Task<TResult>> action, IApizrRequestOptionsBuilder requestOptionsBuilder) =>
            policy.ExecuteAsync((ctx, ct) =>
                {
                    requestOptionsBuilder.WithContext(ctx, ApizrDuplicateStrategy.Replace)
                        .WithCancellation(ct);

                    return action.Invoke(requestOptionsBuilder.ApizrOptions);
                },
                requestOptionsBuilder.ApizrOptions.Context, requestOptionsBuilder.ApizrOptions.CancellationToken);
    }
}
