using System;
using System.Threading;
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
    public bool ClearCache { get; set; }

    /// <inheritdoc />
    public Action<ApizrException> OnException { get; set; }

    /// <inheritdoc />
    public bool LetThrowOnExceptionWithEmptyCache { get; set; }
}