using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring.Shared.Context
{
    public class ApizrResilienceContextOptions : IApizrResilienceContextOptions
    {
        public ApizrResilienceContextOptions(IApizrResilienceContextOptions options = null)
        {
            ContinueOnCapturedContext = options?.ContinueOnCapturedContext;
            ReturnToPoolOnComplete = options?.ReturnToPoolOnComplete;
        }

        /// <inheritdoc />
        public bool? ContinueOnCapturedContext { get; internal set; }

        /// <inheritdoc />
        public bool? ReturnToPoolOnComplete { get; internal set; }
    }
}
