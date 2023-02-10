using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public abstract class ApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder, TApizrRegistryBuilder
    ,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    where TApizrDownloadRegistryBuilder : IApizrDownloadRegistryBuilderBase<TApizrDownloadRegistryBuilder,
        TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
{
    private readonly IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder> _internalBuilder;

    protected ApizrDownloadRegistryBuilderBase(TApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder>;
    }

    protected abstract TApizrDownloadRegistryBuilder Builder { get; }

    /// <inheritdoc />
    public TApizrDownloadRegistryBuilder AddDownloadManager(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        => AddDownloadManagerFor<IDownloadApi>(optionsBuilder);

    /// <inheritdoc />
    public TApizrDownloadRegistryBuilder AddDownloadManagerFor<TDownloadApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi
    {
        if (typeof(TDownloadApi) == typeof(IDownloadApi))
        {
            _internalBuilder?.AddWrappingManagerFor<IDownloadApi, IApizrDownloadManager>(
                apizrManager => new ApizrDownloadManager(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrDownloadManager, IApizrDownloadManager<IDownloadApi>>();
        }
        else
            _internalBuilder?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi>>(
                apizrManager => new ApizrDownloadManager<TDownloadApi>(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        return Builder;
    }

    /// <inheritdoc />
    public TApizrDownloadRegistryBuilder AddDownloadFor<TDownloadApi, TDownloadParams>(
        Action<TApizrProperOptionsBuilder> optionsBuilder = null) where TDownloadApi : IDownloadApi<TDownloadParams>
    {
        _internalBuilder?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi, TDownloadParams>>(
            apizrManager => new ApizrDownloadManager<TDownloadApi, TDownloadParams>(apizrManager),
            optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        return Builder;
    }
}