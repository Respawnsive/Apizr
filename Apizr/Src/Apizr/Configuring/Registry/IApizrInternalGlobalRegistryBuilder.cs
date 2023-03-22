namespace Apizr.Configuring.Registry;

internal interface IApizrInternalGlobalRegistryBuilder
{
    void AddAliasingManagerFor<TAliasingManager, TAliasedManager>();
}