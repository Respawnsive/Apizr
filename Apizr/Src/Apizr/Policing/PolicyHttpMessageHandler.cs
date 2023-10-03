// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Copied from https://github.com/dotnet/extensions/tree/release/3.1/src/HttpClientFactory/Polly/src but without any DI ref and some adjustments

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring.Manager;
using Apizr.Extending;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Policing
{
    /// <summary>
    /// A <see cref="DelegatingHandler"/> implementation that executes request processing surrounded by a <see cref="Policy"/>.
    /// </summary>
    public class PolicyHttpMessageHandler : DelegatingHandler
    {
        private readonly IAsyncPolicy<HttpResponseMessage> _policy;
        private readonly IApizrManagerOptionsBase _apizrOptions;
        private readonly Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> _policySelector;

        /// <summary>
        /// Creates a new <see cref="PolicyHttpMessageHandler"/>.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public PolicyHttpMessageHandler(IAsyncPolicy<HttpResponseMessage> policy, IApizrManagerOptionsBase apizrOptions)
        {
            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            if (apizrOptions == null)
            {
                throw new ArgumentNullException(nameof(apizrOptions));
            }

            _policy = policy;
            _apizrOptions = apizrOptions;
        }

        /// <summary>
        /// Creates a new <see cref="PolicyHttpMessageHandler"/>.
        /// </summary>
        /// <param name="policySelector">A function which can select the desired policy for a given <see cref="HttpRequestMessage"/>.</param>
        public PolicyHttpMessageHandler(Func<HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> policySelector, IApizrManagerOptionsBase apizrOptions)
        {
            if (policySelector == null)
            {
                throw new ArgumentNullException(nameof(policySelector));
            }

            if (apizrOptions == null)
            {
                throw new ArgumentNullException(nameof(apizrOptions));
            }

            _policySelector = policySelector;
            _apizrOptions = apizrOptions;
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
                var context = request.GetApizrPolicyExecutionContext();
                if (context == null)
                {
                    var interfaceType = (Type)request.Properties[Constants.InterfaceTypeKey];
                    context = new Context(interfaceType.Name);
                    request.SetApizrPolicyExecutionContext(context);
                    cleanUpContext = true;
                }

                IAsyncPolicy<HttpResponseMessage> finalPolicy = null;
                var options = request.GetApizrRequestOptions();
                if (options != null)
                {
                    // Get a configured logger instance
                    if (!context.TryGetLogger(out var logger, out var logLevels, out _, out _))
                    {
                        logger = _apizrOptions.Logger;
                        logLevels = _apizrOptions.LogLevels;
                    }

                    IAsyncPolicy<HttpResponseMessage> requestPolicy = null;
                    // Set a request timeout if provided
                    if (options.RequestTimeout.HasValue)
                    {
                        // Set the request timeout
                        if (options.RequestTimeout.Value > TimeSpan.Zero)
                        {
                            requestPolicy = policy.WrapAsync(Policy.TimeoutAsync<HttpResponseMessage>(options.RequestTimeout.Value));
                            logger.Log(logLevels.Low(), "{0}: Timeout has been set with your provided {1} request timeout value.", context.OperationKey, options.RequestTimeout);
                        }
                        else
                        {
                            logger.Log(logLevels.Low(),
                                "{0}: You provided a request timeout value which is not a positive TimeSpan (or Timeout.InfiniteTimeSpan to indicate no timeout). Default value will be applied.",
                                context.OperationKey);
                        }
                    }

                    requestPolicy ??= policy;

                    // Set an operation timeout if provided
                    if (options.OperationTimeout.HasValue)
                    {
                        // Set the request timeout
                        if (options.OperationTimeout.Value > TimeSpan.Zero)
                        {
                            var operationPolicy = Policy.TimeoutAsync<HttpResponseMessage>(options.OperationTimeout.Value);
                            finalPolicy = operationPolicy.WrapAsync(requestPolicy);
                            logger.Log(logLevels.Low(), "{0}: Timeout has been set with your provided {1} operation timeout value.", context.OperationKey, options.OperationTimeout);
                        }
                        else
                        {
                            logger.Log(logLevels.Low(),
                                "{0}: You provided an operation timeout value which is not a positive TimeSpan (or Timeout.InfiniteTimeSpan to indicate no timeout). Default value will be applied.",
                                context.OperationKey);
                        }
                    }

                    finalPolicy ??= requestPolicy;
                }

                finalPolicy ??= policy;

                response = await finalPolicy.ExecuteAsync((c, ct) => SendCoreAsync(request, c, ct), context, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                if (cleanUpContext)
                {
                    request.SetApizrPolicyExecutionContext(null);
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
