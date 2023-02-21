using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Proper;
using Apizr.Transferring.Requesting;

namespace Apizr.Configuring.Registry;

public abstract class ApizrTransferRegistryBuilderBase<TApizrTransferRegistryBuilder, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder,
    TApizrCommonOptionsBuilder> : IApizrTransferRegistryBuilderBase<TApizrTransferRegistryBuilder, TApizrRegistryBuilder,
    TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
    where TApizrRegistryBuilder : IApizrRegistryBuilderBase
    where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
    where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    where TApizrTransferRegistryBuilder : IApizrTransferRegistryBuilderBase<TApizrTransferRegistryBuilder,
        TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
{
    protected abstract TApizrTransferRegistryBuilder Builder { get; }

    /// <inheritdoc />
    public TApizrTransferRegistryBuilder AddTransferManager(Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        => AddTransferManagerFor<ITransferApi>(optionsBuilder);

    /// <inheritdoc />
    public abstract TApizrTransferRegistryBuilder AddTransferManagerFor<TTransferApi>(
        Action<TApizrProperOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi;

    /// <inheritdoc />
    public abstract TApizrTransferRegistryBuilder AddTransferManagerFor<TTransferApi, TDownloadParams>(
        Action<TApizrProperOptionsBuilder> optionsBuilder = null) where TTransferApi : ITransferApi<TDownloadParams>;
}