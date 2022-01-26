// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/dotnet/extensions/tree/release/3.1/src/HttpClientFactory/Polly/src but without any DI ref and some adjustments

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Apizr.Policing
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> implementation that executes request processing surrounded by a <see cref="Policy"/>.
    /// </summary>
    public class PolicyHttpMessageHandler : DelegatingHandler
    {
        private readonly IAsyncPolicy<HttpResponseMessage> _policy;
        private readonly Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> _policySelector;

        /// <summary>
        /// Creates a new <see cref="PolicyHttpMessageHandler"/>.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public PolicyHttpMessageHandler(IAsyncPolicy<HttpResponseMessage> policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            _policy = policy;
        }

        /// <summary>
        /// Creates a new <see cref="PolicyHttpMessageHandler"/>.
        /// </summary>
        /// <param name="policySelector">A function which can select the desired policy for a given <see cref="HttpRequestMessage"/>.</param>
        public PolicyHttpMessageHandler(Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> policySelector)
        {
            if (policySelector == null)
            {
                throw new ArgumentNullException(nameof(policySelector));
            }

            _policySelector = policySelector;
        }
        
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var cleanUpContext = false;
            HttpResponseMessage response;
            try
            {
                var policy = _policy ?? SelectPolicy(request);

                // Guarantee the existence of a context for every policy execution, but only create a new one if needed. This
                // allows later handlers to flow state if desired.
                // We do it right after policy selection so one could set the context during selection
                var context = request.GetPolicyExecutionContext();
                if (context == null)
                {
                    context = new Context();
                    request.SetPolicyExecutionContext(context);
                    cleanUpContext = true;
                }

                response = await policy.ExecuteAsync((c, ct) => SendCoreAsync(request, c, ct), context, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                if (cleanUpContext)
                {
                    request.SetPolicyExecutionContext(null);
                }
            }

            return response;
        }

        /// <summary>
        /// Called inside the execution of the <see cref="Policy"/> to perform request processing.
        /// </summary>
        /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
        /// <param name="context">The <see cref="Context"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>Returns a <see cref="Task{HttpResponseMessage}"/> that will yield a response when completed.</returns>
        protected virtual Task<HttpResponseMessage> SendCoreAsync(HttpRequestMessage request, Context context, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return base.SendAsync(request, cancellationToken);
        }

        private IAsyncPolicy<HttpResponseMessage> SelectPolicy(HttpRequestMessage request)
        {
            var policy = _policySelector(request);
            if (policy == null)
            {
                var message =
                    "The policySelector function must return a non-null policy instance. To create a policy that takes no action, use Policy.NoOpAsync<HttpResponseMessage>().";
                throw new InvalidOperationException(message);
            }

            return policy;
        }
    }
}
