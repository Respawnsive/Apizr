using System;
using Apizr.Configuring.Registry;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Manager;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr;

/// <summary>
/// File transfer options builder extensions
/// </summary>
public static class FileTransferOptionsBuilderExtensions
{
    #region Transfer
    
    #region Single

    /// <summary>
    /// Add an upload manager for IUploadApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddUploadManager(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder) =>
        services.AddUploadManagerFor<IUploadApi>(optionsBuilder);

    /// <summary>
    /// Add an upload manager for the provided upload api derived from IUploadApi
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddUploadManagerFor<TUploadApi>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi =>
        services.AddApizrManagerFor<TUploadApi>(optionsBuilder)
            .AddOrReplaceSingleton(
                typeof(IApizrUploadManager<TUploadApi>),
                serviceProvider =>
                    new ApizrUploadManager<TUploadApi>(serviceProvider
                        .GetRequiredService<IApizrManager<TUploadApi>>()));

    /// <summary>
    /// Add a download manager for IDownloadApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddDownloadManager(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
        services.AddDownloadManagerFor<IDownloadApi>(optionsBuilder);

    /// <summary>
    /// Add a download manager for the provided download api derived from IDownloadApi
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddDownloadManagerFor<TDownloadApi>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi =>
        services.AddApizrManagerFor<TDownloadApi>(optionsBuilder)
            .AddOrReplaceSingleton(
                typeof(IApizrDownloadManager<TDownloadApi>),
                serviceProvider =>
                    new ApizrDownloadManager<TDownloadApi>(serviceProvider
                        .GetRequiredService<IApizrManager<TDownloadApi>>()));

    /// <summary>
    /// Add a download manager for the provided download api derived from IDownloadApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddDownloadManagerFor<TDownloadApi, TDownloadParams>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi<TDownloadParams> =>
        services.AddApizrManagerFor<TDownloadApi>(optionsBuilder)
            .AddOrReplaceSingleton(
                typeof(IApizrDownloadManager<TDownloadApi, TDownloadParams>),
                serviceProvider =>
                    new ApizrDownloadManager<TDownloadApi, TDownloadParams>(serviceProvider
                        .GetRequiredService<IApizrManager<TDownloadApi>>()));

    /// <summary>
    /// Add a transfer manager for ITransferApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddTransferManager(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
        services.AddTransferManagerFor<ITransferApi>(optionsBuilder);

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddTransferManagerFor<TTransferApi>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi =>
        services.AddApizrManagerFor<TTransferApi>(optionsBuilder)
            .AddOrReplaceSingleton(
                typeof(IApizrTransferManager<TTransferApi>),
                serviceProvider =>
                    new ApizrTransferManager<TTransferApi>(serviceProvider
                            .GetRequiredService<IApizrDownloadManager<TTransferApi>>(),
                        serviceProvider
                            .GetRequiredService<IApizrUploadManager<TTransferApi>>()));

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddTransferManagerFor<TTransferApi, TDownloadParams>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi<TDownloadParams> =>
        services.AddApizrManagerFor<TTransferApi>(optionsBuilder)
            .AddOrReplaceSingleton(
                typeof(IApizrTransferManager<TTransferApi, TDownloadParams>),
                serviceProvider =>
                    new ApizrTransferManager<TTransferApi, TDownloadParams>(serviceProvider
                            .GetRequiredService<IApizrDownloadManager<TTransferApi, TDownloadParams>>(),
                        serviceProvider
                            .GetRequiredService<IApizrUploadManager<TTransferApi>>()));

    #endregion

    #region Multiple

    #region Add

    /// <summary>
    /// Add an upload manager for IUploadApi (you must at least provide a base url)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddUploadManager(this IApizrExtendedRegistryBuilder builder,
        Action<IApizrExtendedProperOptionsBuilder> optionsBuilder)
    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrExtendedProperOptionsBuilder> internalBuilder)
            internalBuilder.AddWrappingManagerFor<IUploadApi, IApizrUploadManager<IUploadApi>>(apizrManager => new ApizrUploadManager<IUploadApi>(apizrManager), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Add an upload manager for each provided upload api derived from IUploadApi
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="uploadRegistry">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddUploadManager(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedUploadRegistryBuilder> uploadRegistry,
            Action<IApizrExtendedCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => uploadRegistry.Invoke(
                new ApizrExtendedUploadRegistryBuilder(group)), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Add a download manager for IDownloadApi (you must at least provide a base url)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddDownloadManager(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder)
    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrExtendedProperOptionsBuilder> internalBuilder)
            internalBuilder.AddWrappingManagerFor<IDownloadApi, IApizrDownloadManager<IDownloadApi>>(apizrManager => new ApizrDownloadManager<IDownloadApi>(apizrManager), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Add a download manager for each provided download api derived from IDownloadApi
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="downloadRegistry">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddDownloadManager(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedDownloadRegistryBuilder> downloadRegistry,
            Action<IApizrExtendedCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => downloadRegistry.Invoke(
                new ApizrExtendedDownloadRegistryBuilder(group)), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Add a transfer manager for ITransferApi (you must at least provide a base url)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddTransferManager(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder)
    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrExtendedProperOptionsBuilder> internalBuilder)
            internalBuilder.AddWrappingManagerFor<ITransferApi, IApizrTransferManager<ITransferApi>>(apizrManager =>
                new ApizrTransferManager<ITransferApi>(new ApizrDownloadManager<ITransferApi>(apizrManager),
                    new ApizrUploadManager<ITransferApi>(apizrManager)), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Add a transfer manager for each provided transfer api derived from ITransferApi
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="transferRegistry">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddTransferManager(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedTransferRegistryBuilder> transferRegistry,
            Action<IApizrExtendedCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => transferRegistry.Invoke(
                new ApizrExtendedTransferRegistryBuilder(group)), optionsBuilder);

        return builder;
    }

    #endregion

    #endregion

    #endregion
}