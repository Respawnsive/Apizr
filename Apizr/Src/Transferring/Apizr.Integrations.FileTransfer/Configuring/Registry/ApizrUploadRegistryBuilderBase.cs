using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
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
    protected abstract TApizrUploadRegistryBuilder Builder { get; }

    /// <inheritdoc />
    public TApizrUploadRegistryBuilder AddUploadManager(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        => AddUploadManagerFor<IUploadApi>(optionsBuilder);

    /// <inheritdoc />
    public abstract TApizrUploadRegistryBuilder AddUploadManagerFor<TUploadApi>(
        Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi;
}