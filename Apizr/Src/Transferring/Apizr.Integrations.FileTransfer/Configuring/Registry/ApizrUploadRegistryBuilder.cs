using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public class ApizrUploadRegistryBuilder : ApizrUploadRegistryBuilderBase<IApizrUploadRegistryBuilder, IApizrRegistryBuilder, IApizrProperOptionsBuilder,
    IApizrCommonOptionsBuilder>, IApizrUploadRegistryBuilder
{
    private readonly IApizrInternalRegistryBuilder<IApizrProperOptionsBuilder> _internalBuilder;

    /// <inheritdoc />
    public ApizrUploadRegistryBuilder(IApizrRegistryBuilder builder)
    {
        _internalBuilder = builder as IApizrInternalRegistryBuilder<IApizrProperOptionsBuilder>;
    }

    /// <inheritdoc />
    protected override IApizrUploadRegistryBuilder Builder => this;

    /// <inheritdoc />
    public override IApizrUploadRegistryBuilder AddUploadManagerFor<TUploadApi>(Action<IApizrProperOptionsBuilder> optionsBuilder = null)
    {
        if (typeof(TUploadApi) == typeof(IUploadApi))
        {
            _internalBuilder?.AddWrappingManagerFor<IUploadApi, IApizrUploadManager>(
                apizrManager => new ApizrUploadManager(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            _internalBuilder?.AddAliasingManagerFor<IApizrUploadManager<IUploadApi>, IApizrUploadManager>();
        }
        else
            _internalBuilder?.AddWrappingManagerFor<TUploadApi, IApizrUploadManager<TUploadApi>>(
                apizrManager => new ApizrUploadManager<TUploadApi>(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

        return Builder;
    }
}