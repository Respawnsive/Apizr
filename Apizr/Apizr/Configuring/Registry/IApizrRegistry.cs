using Apizr.Configuring.Common;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistry : IApizrRegistryBase
    {
        IApizrCommonOptions ApizrCommonOptions { get; }
    }
}
