namespace Apizr.Configuring.Registry
{
    public interface IApizrRegistryBuilder<out TApizrRegistry, out TApizrRegistryBuilder> : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder>
        where TApizrRegistry : IApizrRegistryBase
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder>
    {
    }

    public interface IApizrRegistryBuilder : IApizrRegistryBuilder<IApizrRegistry, IApizrRegistryBuilder>
    {}
}
