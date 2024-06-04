using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;
using Apizr.Extending;
using Microsoft.Extensions.Logging;
using Polly.Timeout;
using Polly.Registry;
using System.Collections.Generic;
using System.Linq;
using Apizr.Configuring;

namespace Apizr.Resiliencing
{
    /// <summary>
    /// Base class for resilience handler, i.e. handlers that use resilience strategies to send the requests.
    /// </summary>
    public class ResilienceHttpMessageHandler : DelegatingHandler
    {
        private readonly ResiliencePipelineRegistry<string> _registry;
        private readonly Func<HttpRequestMessage, CancellationToken, ResiliencePipeline<HttpResponseMessage>> _pipelineProvider;
        private readonly IApizrManagerOptionsBase _apizrOptions;

        public ResilienceHttpMessageHandler(ResiliencePipelineRegistry<string> registry, IApizrManagerOptionsBase apizrOptions)
        {
            _registry = registry;
            _apizrOptions = apizrOptions ?? throw new ArgumentNullException(nameof(apizrOptions));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResilienceHttpMessageHandler"/> class.
        /// </summary>
        /// <param name="pipelineProvider">The pipeline provider that supplies pipelines in response to an http message.</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <exception cref="ArgumentNullException">If <paramref name="pipelineProvider"/> is <see langword="null"/>.</exception>
        public ResilienceHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, ResiliencePipeline<HttpResponseMessage>> pipelineProvider, IApizrManagerOptionsBase apizrOptions)
        {
            _pipelineProvider = pipelineProvider ?? throw new ArgumentNullException(nameof(pipelineProvider));
            _apizrOptions = apizrOptions ?? throw new ArgumentNullException(nameof(apizrOptions));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResilienceHttpMessageHandler"/> class.
        /// </summary>
        /// <param name="pipeline">The pipeline to use for the message.</param>
        /// <param name="apizrOptions">The Apizr options</param>
        /// <exception cref="ArgumentNullException">If <paramref name="pipeline"/> is <see langword="null"/>.</exception>
        public ResilienceHttpMessageHandler(ResiliencePipeline<HttpResponseMessage> pipeline, IApizrManagerOptionsBase apizrOptions)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(pipeline);
#else
            if (pipeline == null)
                throw new ArgumentNullException(nameof(pipeline));
#endif
            _pipelineProvider = (_, _) => pipeline;
            _apizrOptions = apizrOptions ?? throw new ArgumentNullException(nameof(apizrOptions));
        }

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="request"/> is <see langword="null"/>.</exception>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(request);
#else
            if (request == null)
                throw new ArgumentNullException(nameof(request));
#endif

            // Get a context from the pool or create a new one
            var created = false;
            if (request.GetApizrResilienceContext() is not { } context)
            {
                context = request.GetOrBuildApizrResilienceContext(cancellationToken);
                created = true;
                request.SetApizrResilienceContext(context);
            }

            // Get a configured logger instance
            if (!context.TryGetLogger(out var logger, out var logLevels, out var verbosity, out var tracerMode))
            {
                if (request.TryGetApizrRequestOptions(out var requestOptions))
                {
                    logLevels = requestOptions.LogLevels;
                    verbosity = requestOptions.TrafficVerbosity;
                    tracerMode = requestOptions.HttpTracerMode;
                }
                else
                {
                    logLevels = _apizrOptions.LogLevels;
                    verbosity = _apizrOptions.TrafficVerbosity;
                    tracerMode = _apizrOptions.HttpTracerMode;
                }
                logger = _apizrOptions.Logger;

                context.WithLogger(logger, logLevels, verbosity, tracerMode);
                request.SetApizrResilienceContext(context);
            }

            context.Properties.Set(Constants.RequestMessagePropertyKey, request);

            var pipelineBuilder = new ResiliencePipelineBuilder<HttpResponseMessage>();
            var options = request.GetApizrRequestOptions() as IApizrGlobalSharedOptionsBase ?? _apizrOptions;

            // Set an operation timeout first if provided
            if (options.OperationTimeout.HasValue)
            {
                // Set the request timeout
                if (options.OperationTimeout.Value > TimeSpan.Zero)
                {
                    pipelineBuilder.AddTimeout(options.OperationTimeout.Value);
                    logger.Log(logLevels.Low(),
                        "{0}: Timeout has been set with your provided {1} operation timeout value.",
                        context.OperationKey, options.OperationTimeout);
                }
                else
                {
                    logger.Log(logLevels.Low(),
                        "{0}: You provided an operation timeout value which is not a positive TimeSpan (or Timeout.InfiniteTimeSpan to indicate no timeout). Default value will be applied.",
                        context.OperationKey);
                }
            }

            // Add HttpResponseMessage resilience pipelines
            var resiliencePipelineKeys = options.ResiliencePipelineKeys
                .Where(kvp => kvp.Key != ApizrConfigurationSource.All)
                .OrderBy(kvp => kvp.Key)
                .SelectMany(kvp => kvp.Value)
                .Distinct()
                .ToArray();
            if (resiliencePipelineKeys.Length > 0)
            {
                if (_registry == null)
                {
                    logger.Log(options.LogLevels.Low(),
                        $"Global strategies: You defined some resilience strategies but didn't register any of it. Global strategies will be ignored for {context.OperationKey}");
                }
                else
                {
                    var includedKeys = new List<string>();
                    if (options.ResiliencePipelineKeys.TryGetValue(ApizrConfigurationSource.All, out var yetIncludedKeys))
                        includedKeys.AddRange(yetIncludedKeys);

                    foreach (var resiliencePipelineKey in resiliencePipelineKeys)
                    {
                        if (_registry.TryGetPipeline<HttpResponseMessage>(resiliencePipelineKey, out var registeredResiliencePipeline))
                        {
                            pipelineBuilder.AddPipeline(registeredResiliencePipeline);
                            includedKeys.Add(resiliencePipelineKey);
                            logger.Log(logLevels.Low(), "{0}: Resilience pipeline with key {1} will be applied", context.OperationKey, resiliencePipelineKey);
                        }
                    }

                    // Report for missing keys
                    options.ResiliencePipelineKeys[ApizrConfigurationSource.All] = includedKeys.ToArray();
                    var ignoredKeys = resiliencePipelineKeys.Except(includedKeys);
                    foreach (var ignoredKey in ignoredKeys)
                    {
                        logger.Log(logLevels.Low(), "{0}: Resilience pipeline with key {1} could not be found.", context.OperationKey, ignoredKey);
                    }
                } 
            }
            
            // Set a request timeout if provided
            if (options.RequestTimeout.HasValue)
            {
                // Set the request timeout
                if (options.RequestTimeout.Value > TimeSpan.Zero)
                {
                    pipelineBuilder.AddTimeout(options.RequestTimeout.Value);
                    logger.Log(logLevels.Low(), "{0}: Timeout has been set with your provided {1} request timeout value.", context.OperationKey, options.RequestTimeout);
                }
                else
                {
                    logger.Log(logLevels.Low(),
                        "{0}: You provided a request timeout value which is not a positive TimeSpan (or Timeout.InfiniteTimeSpan to indicate no timeout). Default value will be applied.",
                        context.OperationKey);
                }
            }

            var pipeline = pipelineBuilder.Build();

            try
            {
                var outcome = await pipeline.ExecuteOutcomeAsync(
                    static async (context, state) =>
                    {
                        var request = context.Properties.GetValue(Constants.RequestMessagePropertyKey, state.request); 
                        var options = request.GetApizrRequestOptions();
                        var optionsCancellationToken = options?.CancellationToken ?? CancellationToken.None;

                        // Always re-assign the context to this request message before execution.
                        // This is because for primary actions the context is also cloned and we need to re-assign it
                        // here because Polly doesn't have any other events that we can hook into.
                        request.SetApizrResilienceContext(context);

                        try
                        {
                            var response = await state.instance.SendCoreAsync(request, context.CancellationToken, optionsCancellationToken, context.ContinueOnCapturedContext).ConfigureAwait(context.ContinueOnCapturedContext);
                            return Outcome.FromResult(response);
                        }
#pragma warning disable CA1031 // Do not catch general exception types
                        catch (Exception e)
                        {
                            return Outcome.FromException<HttpResponseMessage>(e);
                        }
#pragma warning restore CA1031 // Do not catch general exception types
                    },
                    context,
                    (instance: this, request))
                    .ConfigureAwait(context.ContinueOnCapturedContext);

                outcome.ThrowIfException();

                return outcome.Result!;
            }
            finally
            {
                if (created)
                {
                    ResilienceContextPool.Shared.Return(context);
                    request.SetApizrResilienceContext(null);
                }
                else
                {
                    // Restore the original context
                    request.SetApizrResilienceContext(context);
                }
            }
        }

        private async Task<HttpResponseMessage> SendCoreAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken, CancellationToken optionsCancellationToken, bool continueOnCapturedContext)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(requestMessage);
#else
            if (requestMessage == null)
                throw new ArgumentNullException(nameof(requestMessage));
#endif

            try
            {
                return await base.SendAsync(requestMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext);
            }
            catch (WebException ex)
                when (cancellationToken.IsCancellationRequested &&
                      !optionsCancellationToken.IsCancellationRequested) // Actually a timeout cancellation
            {
                throw new TimeoutRejectedException(ex.Message, ex);
            }
        }
    }
}
