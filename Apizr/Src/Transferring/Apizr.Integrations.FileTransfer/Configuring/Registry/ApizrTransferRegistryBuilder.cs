using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public class ApizrTransferRegistryBuilder : ApizrTransferRegistryBuilderBase<IApizrTransferRegistryBuilder,
    IApizrRegistryBuilder, IApizrProperOptionsBuilder,
    IApizrCommonOptionsBuilder>, IApizrTransferRegistryBuilder
{
    private readonly IApizrInternalRegistryBuilder<IApizrProperOptionsBuilder> _internalBuilder;

    /// <inheritdoc />
    public ApizrTransferRegistryBuilder(IApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilder<IApizrProperOptionsBuilder>;
    }

    /// <inheritdoc />
    protected override IApizrTransferRegistryBuilder Builder => this;

    /// <inheritdoc />
    public override IApizrTransferRegistryBuilder AddTransferManagerFor<TTransferApi>(
        Action<IApizrProperOptionsBuilder> optionsBuilder = null)
    {
        if (typeof(TTransferApi) == typeof(ITransferApi))
        {
            // Upload
            _internalBuilder?.AddWrappingManagerFor<IUploadApi, IApizrUploadManager>(
                apizrManager => new ApizrUploadManager(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrUploadManager<IUploadApi>, IApizrUploadManager>();

            // Download
            _internalBuilder?.AddWrappingManagerFor<IDownloadApi, IApizrDownloadManager>(
                apizrManager => new ApizrDownloadManager(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrDownloadManager<IDownloadApi>, IApizrDownloadManager>();

            // Transfer
            _internalBuilder?.AddWrappingManagerFor<ITransferApi, IApizrTransferManager>(
                apizrManager => new ApizrTransferManager(
                    new ApizrDownloadManager<ITransferApi>(apizrManager),
                    new ApizrUploadManager<ITransferApi>(apizrManager)),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrTransferManager<ITransferApi>, IApizrTransferManager>();
        }
        else
        {
            // Upload
            _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrUploadManager<TTransferApi>>(
                apizrManager => new ApizrUploadManager<TTransferApi>(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            // Download
            _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrDownloadManager<TTransferApi>>(
                apizrManager => new ApizrDownloadManager<TTransferApi>(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            // Transfer
            _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi>>(
                apizrManager => new ApizrTransferManager<TTransferApi>(
                    new ApizrDownloadManager<TTransferApi>(apizrManager),
                    new ApizrUploadManager<TTransferApi>(apizrManager)),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));
        }

        return Builder;
    }

    /// <inheritdoc />
    public override IApizrTransferRegistryBuilder AddTransferManagerFor<TTransferApi, TDownloadParams>(
        Action<IApizrProperOptionsBuilder> optionsBuilder = null)
    {
        // Upload
        _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrUploadManager<TTransferApi>>(
            apizrManager => new ApizrUploadManager<TTransferApi>(apizrManager),
            optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

        // Download
        _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrDownloadManager<TTransferApi, TDownloadParams>>(
            apizrManager => new ApizrDownloadManager<TTransferApi, TDownloadParams>(apizrManager),
            optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        // Transfer
        _internalBuilder?.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi, TDownloadParams>>(
            apizrManager => new ApizrTransferManager<TTransferApi, TDownloadParams>(
                new ApizrDownloadManager<TTransferApi, TDownloadParams>(apizrManager),
                new ApizrUploadManager<TTransferApi>(apizrManager)),
            optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

        return Builder;
    }
}