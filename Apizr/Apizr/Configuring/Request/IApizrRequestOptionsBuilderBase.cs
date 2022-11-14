namespace Apizr.Configuring.Request
{
    public interface IApizrRequestOptionsBuilderBase<out TApizrRequestOptions, out TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrRequestOptionsBase
        where TApizrRequestOptionsBuilder : IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
    }

    public interface IApizrRequestOptionsBuilderBase : IApizrRequestOptionsBuilderBase<IApizrRequestOptionsBase,
        IApizrRequestOptionsBuilderBase>
    {
        internal IApizrRequestOptions ApizrOptions { get; }
    }

    public interface IApizrResultRequestOptionsBuilderBase : IApizrRequestOptionsBuilderBase
    {

    }
}
