using Apizr.Configuring.Common;

namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistryBuilderBase
    {
    }

    public interface IApizrRegistryBuilderBase<out TApizrRegistry, out TApizrRegistryBuilder> : IApizrCommonOptionsBuilderBase
        where TApizrRegistry : IApizrRegistryBase
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder>
    {
        /// <summary>
        /// Apizr registry
        /// </summary>
        TApizrRegistry ApizrRegistry { get; }
    }
}
