using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public class ApizrDownloadRegistryBuilder : ApizrDownloadRegistryBuilderBase<IApizrDownloadRegistryBuilder, IApizrRegistryBuilder, IApizrProperOptionsBuilder,
    IApizrCommonOptionsBuilder>, IApizrDownloadRegistryBuilder
{
    private readonly IApizrInternalRegistryBuilder<IApizrProperOptionsBuilder> _internalBuilder;

    /// <inheritdoc />
    public ApizrDownloadRegistryBuilder(IApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilder<IApizrProperOptionsBuilder>;
    }

    /// <inheritdoc />
    protected override IApizrDownloadRegistryBuilder Builder => this;

    /// <inheritdoc />
    public override IApizrDownloadRegistryBuilder AddDownloadManagerFor<TDownloadApi>(Action<IApizrProperOptionsBuilder> optionsBuilder = null)
    {
        if (typeof(TDownloadApi) == typeof(IDownloadApi))
        {
            _internalBuilder?.AddWrappingManagerFor<IDownloadApi, IApizrDownloadManager>(
                apizrManager => new ApizrDownloadManager(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrDownloadManager<IDownloadApi>, IApizrDownloadManager>();
        }
        else
            _internalBuilder?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi>>(
                apizrManager => new ApizrDownloadManager<TDownloadApi>(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        return Builder;
    }

    /// <inheritdoc />
    public override IApizrDownloadRegistryBuilder AddDownloadFor<TDownloadApi, TDownloadParams>(Action<IApizrProperOptionsBuilder> optionsBuilder = null)
    {
        _internalBuilder?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi, TDownloadParams>>(
            apizrManager => new ApizrDownloadManager<TDownloadApi, TDownloadParams>(apizrManager),
            optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        return Builder;
    }
}