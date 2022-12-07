// Copied from Fusillade official repo and then adjusted to Apizr needs

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Request;
using Apizr.Extending;
using Apizr.Policing;
using Fusillade;
using Microsoft.Extensions.Logging;
using Punchclock;

namespace Apizr
{
    public class PriorityHttpMessageHandler : LimitingHttpMessageHandler
    {
        private readonly OperationQueue _opQueue;
        private readonly Dictionary<string, InflightRequest> _inflightResponses = new Dictionary<string, InflightRequest>();
        private readonly ILogger _logger;
        private readonly IApizrManagerOptionsBase _apizrOptions;
        private long? _maxBytesToRead;

        public PriorityHttpMessageHandler(HttpMessageHandler innerHandler, ILogger logger, IApizrManagerOptionsBase apizrOptions, long? maxBytesToRead = null, OperationQueue opQueue = null) : base(innerHandler)
        {
            _logger = logger;
            _apizrOptions = apizrOptions;
            _maxBytesToRead = maxBytesToRead;
            _opQueue = opQueue;
        }

        public override void ResetLimit(long? maxBytesToRead = null)
        {
            _maxBytesToRead = maxBytesToRead;
        }

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Doesn't need to be disposed.")]
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var method = request.Method;
            if (method != HttpMethod.Get && method != HttpMethod.Head && method != HttpMethod.Options)
            {
                return base.SendAsync(request, cancellationToken);
            }

            if (_maxBytesToRead != null && _maxBytesToRead.Value < 0)
            {
                var tcs = new TaskCompletionSource<HttpResponseMessage>();
                tcs.SetCanceled();
                return tcs.Task;
            }

            var context = request.GetOrBuildApizrPolicyExecutionContext();
            if (!context.TryGetLogger(out var logger, out var logLevels, out var verbosity, out var tracerMode))
            {
                if (request.TryGetOptions(out var requestOptions))
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
                logger = _logger;

                context.WithLogger(logger, logLevels, verbosity, tracerMode);
                request.SetApizrPolicyExecutionContext(context);
            }

            var priority = (int) Priority.UserInitiated;
            if (request.Properties.TryGetValue(Constants.PriorityKey, out var priorityObject) &&
                priorityObject is int priorityValue and >= 0)
            {
                priority = priorityValue;
            }
            else if (request.Properties.TryGetValue(Constants.ApizrRequestOptionsKey, out var optionsObject) &&
                     optionsObject is IApizrRequestOptions options &&
                     options.HandlersParameters.TryGetValue(Constants.PriorityKey, out priorityObject) &&
                     priorityObject is int optionsPriorityValue and >= 0)
            {
                priority = optionsPriorityValue;
            }

            var priorityName = Enum.IsDefined(typeof(Priority), priority)
                ? $"a {Enum.GetName(typeof(Priority), priority)} priority"
                : $"a custom priority of {priority}";
            var key = RateLimitedHttpMessageHandler.UniqueKeyForRequest(request);
            var realToken = new CancellationTokenSource();
            var ret = new InflightRequest(() =>
            {
                lock (_inflightResponses)
                {
                    _inflightResponses.Remove(key);
                }

                realToken.Cancel();
            });

            lock (_inflightResponses)
            {
                if (_inflightResponses.ContainsKey(key))
                {
                    var val = _inflightResponses[key];
                    val.AddRef();
                    cancellationToken.Register(val.Cancel);

                    logger.Log(logLevels.Low(), $"{context.OperationKey}: Same request has been sent yet. Waiting for it.");

                    return val.Response.ToTask(cancellationToken);
                }

                _inflightResponses[key] = ret;
            }

            cancellationToken.Register(ret.Cancel);

            var queue = _opQueue ?? NetCache.OperationQueue;

            queue.Enqueue(
                priority,
                null,
                async () =>
                {
                    try
                    {
                        logger.Log(logLevels.Low(), $"{context.OperationKey}: Sending request with {priorityName}.");

                        var resp = await base.SendAsync(request, realToken.Token).ConfigureAwait(false);

                        if (_maxBytesToRead != null && resp.Content?.Headers.ContentLength != null)
                        {
                            _maxBytesToRead -= resp.Content.Headers.ContentLength;
                        }

                        return resp;
                    }
                    finally
                    {
                        lock (_inflightResponses)
                        {
                            _inflightResponses.Remove(key);
                        }
                    }
                },
                realToken.Token).ToObservable().Subscribe(ret.Response);

            return ret.Response.ToTask(cancellationToken);
        }
    }
}
