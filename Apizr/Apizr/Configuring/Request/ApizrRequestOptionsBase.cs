using Apizr.Configuring.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apizr.Configuring.Request
{
    public abstract class ApizrRequestOptionsBase : ApizrGlobalSharedOptionsBase, IApizrRequestOptionsBase
    {
        /// <inheritdoc />
        protected ApizrRequestOptionsBase(IApizrGlobalSharedOptionsBase sharedOptions) : base(sharedOptions)
        {
        }
    }
}
