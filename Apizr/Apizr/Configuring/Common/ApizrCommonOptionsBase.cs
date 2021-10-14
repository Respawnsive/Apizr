using Apizr.Configuring.Shared;
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
