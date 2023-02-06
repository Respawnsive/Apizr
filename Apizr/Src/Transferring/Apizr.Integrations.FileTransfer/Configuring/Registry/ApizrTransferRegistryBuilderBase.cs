using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public abstract class ApizrTransferRegistryBuilderBase<TApizrTransferRegistryBuilder, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrTransferRegistryBuilderBase<TApizrTransferRegistryBuilder, TApizrRegistryBuilder
    ,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    where TApizrTransferRegistryBuilder : IApizrTransferRegistryBuilderBase<TApizrTransferRegistryBuilder,
        TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
{
    private readonly IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder> _internalBuilder;

    protected ApizrTransferRegistryBuilderBase(TApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilderBase<TApizrProperOptionsBuilder>;
    }

    protected abstract TApizrTransferRegistryBuilder Builder { get; }

    /// <inheritdoc />
    public TApizrTransferRegistryBuilder AddFor<TTransferApi>(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi
    {
        _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi>>(
            apizrManager => new ApizrTransferManager<TTransferApi>(new ApizrDownloadManager<TTransferApi>(apizrManager),
                new ApizrUploadManager<TTransferApi>(apizrManager)),
            FileTransferOptionsBuilderExtensions.IgnoreMessageParts(optionsBuilder,
                HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

        return Builder;
    }

    /// <inheritdoc />
    public TApizrTransferRegistryBuilder AddFor<TTransferApi, TDownloadParams>(
        Action<TApizrProperOptionsBuilder> optionsBuilder = null) where TTransferApi : ITransferApi<TDownloadParams>
    {
        _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi, TDownloadParams>>(
            apizrManager => new ApizrTransferManager<TTransferApi, TDownloadParams>(
                new ApizrDownloadManager<TTransferApi, TDownloadParams>(apizrManager),
                new ApizrUploadManager<TTransferApi>(apizrManager)),
            FileTransferOptionsBuilderExtensions.IgnoreMessageParts(optionsBuilder,
                HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

        return Builder;
    }
}