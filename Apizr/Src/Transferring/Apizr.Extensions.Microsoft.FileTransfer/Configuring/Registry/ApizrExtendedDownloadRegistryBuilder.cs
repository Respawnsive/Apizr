using System;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public class ApizrExtendedDownloadRegistryBuilder : ApizrDownloadRegistryBuilderBase<
    IApizrExtendedDownloadRegistryBuilder, IApizrExtendedRegistryBuilder, IApizrExtendedProperOptionsBuilder,
    IApizrExtendedCommonOptionsBuilder>, IApizrExtendedDownloadRegistryBuilder
{
    private readonly IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> _internalBuilder;

    /// <inheritdoc />
    public ApizrExtendedDownloadRegistryBuilder(IApizrExtendedRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder>;
    }

    /// <inheritdoc />
    protected override IApizrExtendedDownloadRegistryBuilder Builder => this;

    /// <inheritdoc />
    public override IApizrExtendedDownloadRegistryBuilder AddDownloadManagerFor<TDownloadApi>(
        Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
    {
        if (typeof(TDownloadApi) == typeof(IDownloadApi))
        {
            _internalBuilder?.AddWrappingManagerFor<IDownloadApi, IApizrDownloadManager, ApizrDownloadManager>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrDownloadManager<IDownloadApi> , IApizrDownloadManager>();
        }
        else
            _internalBuilder
                ?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi>,
                    ApizrDownloadManager<TDownloadApi>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        return Builder;
    }

    /// <inheritdoc />
    public override IApizrExtendedDownloadRegistryBuilder AddDownloadFor<TDownloadApi, TDownloadParams>(
        Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
    {
        _internalBuilder
            ?.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi, TDownloadParams>,
                ApizrDownloadManager<TDownloadApi, TDownloadParams>>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        return Builder;
    }
}