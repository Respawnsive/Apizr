using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Logging;
using Fusillade;
using Punchclock;

namespace Apizr.Integrations.Fusillade
{
    public class PriorityHttpMessageHandler : LimitingHttpMessageHandler
    {
        private readonly OperationQueue _opQueue;
        private readonly Dictionary<string, InflightRequest> _inflightResponses = new Dictionary<string, InflightRequest>();
        private readonly ILogHandler _logHandler;
        private long? _maxBytesToRead;

        public PriorityHttpMessageHandler(HttpMessageHandler innerHandler, ILogHandler logHandler, long? maxBytesToRead = null, OperationQueue opQueue = null) : base(innerHandler)
        {
            _logHandler = logHandler;
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

            var priority = (int) Priority.UserInitiated;
            if (request.Properties.TryGetValue("Priority", out var priorityObject))
            {
                var priorityValue = (int) priorityObject;
                if (priorityValue >= 0)
                    priority = priorityValue;
            }

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
