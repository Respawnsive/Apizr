using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public abstract class ApizrUploadRegistryBuilderBase<TApizrUploadRegistryBuilder, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrUploadRegistryBuilderBase<TApizrUploadRegistryBuilder, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    where TApizrUploadRegistryBuilder : IApizrUploadRegistryBuilderBase<TApizrUploadRegistryBuilder,
        TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
{
    private readonly IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder> _internalBuilder;

    protected ApizrUploadRegistryBuilderBase(TApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder>;
    }

    protected abstract TApizrUploadRegistryBuilder Builder { get; }

    /// <inheritdoc />
    public TApizrUploadRegistryBuilder AddUploadManager(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        => AddUploadManagerFor<IUploadApi>(optionsBuilder);

    /// <inheritdoc />
    public TApizrUploadRegistryBuilder AddUploadManagerFor<TUploadApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi
    {
        if (typeof(TUploadApi) == typeof(IUploadApi))
        {
            _internalBuilder?.AddWrappingManagerFor<IUploadApi, IApizrUploadManager>(
                apizrManager => new ApizrUploadManager(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrUploadManager, IApizrUploadManager<IUploadApi>>();
        }
        else
            _internalBuilder?.AddWrappingManagerFor<TUploadApi, IApizrUploadManager<TUploadApi>>(
                apizrManager => new ApizrUploadManager<TUploadApi>(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

        return Builder;
    }
}