using System;
using System.Linq;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Configuring.Common
{
    public abstract class ApizrCommonOptionsBase : ApizrSharedOptionsBase, IApizrCommonOptionsBase
    {
        protected ApizrCommonOptionsBase()
        {
        }

        public IHttpContentSerializer ContentSerializer { get; protected set; }
    }
}
