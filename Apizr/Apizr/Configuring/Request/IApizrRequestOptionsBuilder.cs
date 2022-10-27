using System;
using System.Threading;
using Polly;

namespace Apizr.Configuring.Request
{
    #region Builders

    public interface IApizrContextRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
    IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    where TApizrRequestOptions : IApizrContextRequestOption
    where TApizrRequestOptionsBuilder : IApizrContextRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        TApizrRequestOptionsBuilder WithContext(Context context);
    }

    public interface IApizrCancellationRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
        IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrCancellationRequestOption
        where TApizrRequestOptionsBuilder : IApizrCancellationRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        TApizrRequestOptionsBuilder WithCancellation(CancellationToken cancellationToken);
    }

    public interface IApizrCacheRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
        IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrCacheRequestOption
        where TApizrRequestOptionsBuilder : IApizrCacheRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        TApizrRequestOptionsBuilder ClearCache(bool clearCache);
    }

    public interface IApizrCatchRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
        IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrCatchRequestOption
        where TApizrRequestOptionsBuilder : IApizrCatchRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        TApizrRequestOptionsBuilder Catch(Action<ApizrException> onException);
    }

    #endregion

    #region Combinations

    #region Unit

    public interface IApizrUnitRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
    IApizrContextRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>,
    IApizrCancellationRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    where TApizrRequestOptions : IApizrUnitRequestOptions
    where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
    }

    public interface IApizrUnitRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<IApizrUnitRequestOptions, IApizrUnitRequestOptionsBuilder>
    { }

    #endregion

    #region CatchUnit

    public interface IApizrCatchUnitRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
    IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>,
    IApizrCatchRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    where TApizrRequestOptions : IApizrCatchUnitRequestOptions
    where TApizrRequestOptionsBuilder : IApizrCatchUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
    }

    public interface IApizrCatchUnitRequestOptionsBuilder : IApizrCatchUnitRequestOptionsBuilder<IApizrCatchUnitRequestOptions, IApizrCatchUnitRequestOptionsBuilder>
    { }

    #endregion

    #region Result

    public interface IApizrResultRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
        IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IApizrCacheRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
    }

    public interface IApizrResultRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<IApizrResultRequestOptions, IApizrResultRequestOptionsBuilder>
    { }

    #endregion

    #region CatchResult

    public interface IApizrCatchResultRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
        IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IApizrCatchRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrCatchResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrCatchResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
    }

    public interface IApizrCatchResultRequestOptionsBuilder : IApizrCatchResultRequestOptionsBuilder<IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>
    { }

    #endregion

    #endregion
}
