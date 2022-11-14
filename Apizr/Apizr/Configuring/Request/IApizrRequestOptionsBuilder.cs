using System;
using System.Threading;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Microsoft.Extensions.Logging;
using Polly;

namespace Apizr.Configuring.Request
{
    #region Builders

    public interface IApizrSharedRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
    IApizrRequestOptionsBuilderBase<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    where TApizrRequestOptions : IApizrSharedRequestOptions
    where TApizrRequestOptionsBuilder : IApizrSharedRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        TApizrRequestOptionsBuilder WithContext(Context context);

        TApizrRequestOptionsBuilder CancelWith(CancellationToken cancellationToken);

        TApizrRequestOptionsBuilder AddHandlersParameter(string key, object value);
        
        /// <summary>
        /// Configure logging level for the request
        /// </summary>
        /// <param name="httpTracerMode"></param>
        /// <param name="trafficVerbosity">Http traffic tracing verbosity (default: All)</param>
        /// <param name="logLevels">Log levels to apply while writing (default: Information)</param>
        /// <returns></returns>
        TApizrRequestOptionsBuilder WithLogging(HttpTracerMode httpTracerMode = HttpTracerMode.Everything,
            HttpMessageParts trafficVerbosity = HttpMessageParts.All, params LogLevel[] logLevels);
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
    IApizrSharedRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    where TApizrRequestOptions : IApizrUnitRequestOptions
    where TApizrRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
    }

    public interface IApizrUnitRequestOptionsBuilder : IApizrUnitRequestOptionsBuilder<IApizrUnitRequestOptions, IApizrUnitRequestOptionsBuilder>, IApizrRequestOptionsBuilderBase
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

    public interface IApizrCatchUnitRequestOptionsBuilder : IApizrCatchUnitRequestOptionsBuilder<IApizrCatchUnitRequestOptions, IApizrCatchUnitRequestOptionsBuilder>, IApizrRequestOptionsBuilderBase
    { }

    #endregion

    #region Result

    public interface IApizrResultRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
        IApizrSharedRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IApizrCacheRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
    }

    public interface IApizrResultRequestOptionsBuilder : IApizrResultRequestOptionsBuilder<IApizrResultRequestOptions, IApizrResultRequestOptionsBuilder>, IApizrResultRequestOptionsBuilderBase
    { }

    #endregion

    #region CatchResult

    public interface IApizrCatchResultRequestOptionsBuilder<out TApizrRequestOptions, out TApizrRequestOptionsBuilder> :
        IApizrResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>,
        IApizrCatchRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
        where TApizrRequestOptions : IApizrCatchResultRequestOptions
        where TApizrRequestOptionsBuilder : IApizrCatchResultRequestOptionsBuilder<TApizrRequestOptions, TApizrRequestOptionsBuilder>
    {
        TApizrRequestOptionsBuilder Catch(Action<ApizrException> onException, bool letThrowOnExceptionWithEmptyCache);
    }

    public interface IApizrCatchResultRequestOptionsBuilder : IApizrCatchResultRequestOptionsBuilder<IApizrCatchResultRequestOptions, IApizrCatchResultRequestOptionsBuilder>, IApizrResultRequestOptionsBuilderBase
    { }

    #endregion

    #endregion
}
