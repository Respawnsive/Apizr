using System;
using System.Collections.Generic;
using System.Threading;
using Apizr.Configuring.Shared;
using Polly;

namespace Apizr.Configuring.Request;

public class ApizrRequestOptions : ApizrRequestOptionsBase, IApizrRequestOptions
{
    public ApizrRequestOptions(IApizrGlobalSharedRegistrationOptionsBase sharedOptions) : base(sharedOptions)
    {
    }

    /// <inheritdoc />
    public CancellationToken CancellationToken { get; internal set; }

    /// <inheritdoc />
    public bool ClearCache { get; internal set; }
}