using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Apizr.Configuring.Request
{
    public interface IApizrRequestOptions
    {
        Context Context { get; }

        CancellationToken CancellationToken { get; }

        bool ClearCache { get; }

        Action<Exception> OnException { get; }
    }
}
