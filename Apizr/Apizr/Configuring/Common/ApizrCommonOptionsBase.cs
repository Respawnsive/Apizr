using Apizr.Configuring.Shared;
using Refit;

namespace Apizr.Configuring.Common
{
    public abstract class ApizrCommonOptionsBase : ApizrSharedOptionsBase, IApizrCommonOptionsBase
    {
        protected ApizrCommonOptionsBase()
        {
        }
        
        public RefitSettings RefitSettings { get; protected set; }
    }
}
