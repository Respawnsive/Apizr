using Apizr.Configuring.Common;

namespace Apizr.Configuring.Registry
{
    public class ApizrRegistry : ApizrRegistryBase, IApizrRegistry
    {
        internal ApizrRegistry(IApizrCommonOptions commonOptions)
        {
            ApizrCommonOptions = commonOptions;
        }

        public IApizrCommonOptions ApizrCommonOptions { get; }
    }
}
