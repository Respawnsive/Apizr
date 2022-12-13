using System;
using System.Collections.Generic;
using System.Threading;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request;

public class ApizrRequestOptions : ApizrRequestOptionsBase, IApizrRequestOptions
{
    public ApizrRequestOptions(IApizrGlobalSharedRegistrationOptionsBase sharedOptions,
        HttpTracerMode? httpTracerMode,
        HttpMessageParts? trafficVerbosity,
        params LogLevel[] logLevels) : 
        base(sharedOptions, httpTracerMode, trafficVerbosity, logLevels)
    {
    }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; internal set; }

    /// <inheritdoc />
    public bool ClearCache { get; internal set; }
}