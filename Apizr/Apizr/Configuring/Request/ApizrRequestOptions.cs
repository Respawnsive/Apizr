using System;
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
        Context = new Context();
        CancellationToken = CancellationToken.None;
        ClearCache = false;
        OnException = null;
        LetThrowOnExceptionWithEmptyCache = true;
    }

    /// <inheritdoc />
    public Context Context { get; set; }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; set; }

    /// <inheritdoc />
    public HttpTracerMode HttpTracerMode { get; set; }

    /// <inheritdoc />
    public HttpMessageParts TrafficVerbosity { get; set; }
    
    private LogLevel[] _logLevels;
    /// <inheritdoc />
    public LogLevel[] LogLevels
    {
        get => _logLevels;
        set => _logLevels = value?.Any() == true
            ? value
            : new[]
            {
                Constants.LowLogLevel,
                Constants.MediumLogLevel,
                Constants.HighLogLevel
            };
    }

    /// <inheritdoc />
    public bool ClearCache { get; set; }

    /// <inheritdoc />
    public Action<ApizrException> OnException { get; set; }

    /// <inheritdoc />
    public bool LetThrowOnExceptionWithEmptyCache { get; set; }
}