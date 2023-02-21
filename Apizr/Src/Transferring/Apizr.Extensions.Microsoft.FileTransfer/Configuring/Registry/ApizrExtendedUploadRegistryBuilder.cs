using System;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public class ApizrExtendedUploadRegistryBuilder : ApizrUploadRegistryBuilderBase<IApizrExtendedUploadRegistryBuilder, IApizrExtendedRegistryBuilder, IApizrExtendedProperOptionsBuilder,
    IApizrExtendedCommonOptionsBuilder>, IApizrExtendedUploadRegistryBuilder
{
    private readonly IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> _internalBuilder;

    /// <inheritdoc />
    public ApizrExtendedUploadRegistryBuilder(IApizrExtendedRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder>;
    }

    /// <inheritdoc />
    protected override IApizrExtendedUploadRegistryBuilder Builder => this;

    /// <inheritdoc />
    public override IApizrExtendedUploadRegistryBuilder AddUploadManagerFor<TUploadApi>(Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
    {
        if (typeof(TUploadApi) == typeof(IUploadApi))
        {
            _internalBuilder?.AddWrappingManagerFor<IUploadApi, IApizrUploadManager, ApizrUploadManager>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrUploadManager<IUploadApi> , IApizrUploadManager>();
        }
        else
            _internalBuilder?.AddWrappingManagerFor<TUploadApi, IApizrUploadManager<TUploadApi>, ApizrUploadManager<TUploadApi>>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

        return Builder;
    }
}