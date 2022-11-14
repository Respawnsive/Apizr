using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request;

public class ApizrRequestOptions : IApizrRequestOptions
{
    public ApizrRequestOptions()
    {
        Context = null;
        CancellationToken = CancellationToken.None;
        HandlersParameters = new Dictionary<string, object>();
        ClearCache = false;
        OnException = null;
        LetThrowOnExceptionWithEmptyCache = true;
    }

    /// <inheritdoc />
    public Context Context { get; internal set; }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; internal set; }

    /// <inheritdoc />
    public HttpTracerMode HttpTracerMode { get; internal set; }

    /// <inheritdoc />
    public HttpMessageParts TrafficVerbosity { get; internal set; }
    
    private LogLevel[] _logLevels;
    /// <inheritdoc />
    public LogLevel[] LogLevels
    {
        get => _logLevels;
        internal set => _logLevels = value?.Any() == true
            ? value
            : new[]
            {
                Constants.LowLogLevel,
                Constants.MediumLogLevel,
                Constants.HighLogLevel
            };
    }

    /// <inheritdoc />
    public IDictionary<string, object> HandlersParameters { get; }

    /// <inheritdoc />
    public bool ClearCache { get; internal set; }

    /// <inheritdoc />
    public Action<ApizrException> OnException { get; internal set; }

    /// <inheritdoc />
    public bool LetThrowOnExceptionWithEmptyCache { get; internal set; }
}