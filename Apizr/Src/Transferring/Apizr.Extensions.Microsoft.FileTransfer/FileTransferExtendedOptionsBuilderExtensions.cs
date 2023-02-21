using System;
using Apizr.Configuring.Registry;
using Apizr.Extending.Configuring.Common;
using Apizr.Extending.Configuring.Manager;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
using Microsoft.Extensions.DependencyInjection;

namespace Apizr;

/// <summary>
/// File transfer options builder extensions
/// </summary>
public static class FileTransferExtendedOptionsBuilderExtensions
{
    #region Transfer

    #region Single

    /// <summary>
    /// Add an upload manager for IUploadApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrUploadManager(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder) =>
        services.AddApizrUploadManagerFor<IUploadApi>(optionsBuilder);

    /// <summary>
    /// Add an upload manager for the provided upload api derived from IUploadApi
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrUploadManagerFor<TUploadApi>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi
    {
        services.AddApizrManagerFor<TUploadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        if (typeof(TUploadApi) == typeof(IUploadApi))
            services.TryAddSingleton<IApizrUploadManager, ApizrUploadManager>()
                .TryAddSingleton<IApizrUploadManager<IUploadApi>>(serviceProvider =>
                    serviceProvider.GetRequiredService<IApizrUploadManager>());
        else
            services.TryAddSingleton<IApizrUploadManager<TUploadApi>, ApizrUploadManager<TUploadApi>>();

        return services;
    }

    /// <summary>
    /// Add a download manager for IDownloadApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrDownloadManager(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
        services.AddApizrDownloadManagerFor<IDownloadApi>(optionsBuilder);

    /// <summary>
    /// Add a download manager for the provided download api derived from IDownloadApi
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrDownloadManagerFor<TDownloadApi>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi
    {
        services.AddApizrManagerFor<TDownloadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

        if (typeof(TDownloadApi) == typeof(IDownloadApi))
            services.TryAddSingleton<IApizrDownloadManager, ApizrDownloadManager>()
                .TryAddSingleton<IApizrDownloadManager<IDownloadApi>>(serviceProvider =>
                    serviceProvider.GetRequiredService<IApizrDownloadManager>());
        else
            services.TryAddSingleton<IApizrDownloadManager<TDownloadApi>, ApizrDownloadManager<TDownloadApi>>();

        return services;
    }

    /// <summary>
    /// Add a download manager for the provided download api derived from IDownloadApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrDownloadManagerFor<TDownloadApi, TDownloadParams>(
        this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi<TDownloadParams> =>
        services.AddApizrManagerFor<TDownloadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody))
            .TryAddSingleton<IApizrDownloadManager<TDownloadApi, TDownloadParams>, ApizrDownloadManager<TDownloadApi, TDownloadParams>>();

    /// <summary>
    /// Add a transfer manager for ITransferApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrTransferManager(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
        services.AddApizrTransferManagerFor<ITransferApi>(optionsBuilder);

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrTransferManagerFor<TTransferApi>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi
    {
        services.AddApizrManagerFor<TTransferApi>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody))
            .AddApizrDownloadManagerFor<TTransferApi>()
            .AddApizrUploadManagerFor<TTransferApi>();

        if (typeof(TTransferApi) == typeof(ITransferApi))
            services.TryAddSingleton<IApizrTransferManager, ApizrTransferManager>()
                .TryAddSingleton<IApizrTransferManager<ITransferApi>>(serviceProvider =>
                    serviceProvider.GetRequiredService<IApizrTransferManager>());
        else
            services.TryAddSingleton<IApizrTransferManager<TTransferApi>, ApizrTransferManager<TTransferApi>>();

        return services;
    }

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrTransferManagerFor<TTransferApi, TDownloadParams>(
        this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi<TDownloadParams> =>
        services.AddApizrManagerFor<TTransferApi>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody))
            .AddApizrDownloadManagerFor<TTransferApi, TDownloadParams>()
            .AddApizrUploadManagerFor<TTransferApi>()
            .TryAddSingleton<IApizrTransferManager<TTransferApi, TDownloadParams>,
                ApizrTransferManager<TTransferApi, TDownloadParams>>();

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
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            internalBuilder.AddWrappingManagerFor<IUploadApi, IApizrUploadManager, ApizrUploadManager>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            internalBuilder.AddAliasingManagerFor<IApizrUploadManager<IUploadApi>, IApizrUploadManager>();
        }

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
        AddUploadGroup(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedUploadRegistryBuilder> uploadRegistry,
            Action<IApizrExtendedCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => uploadRegistry.Invoke(
                new ApizrExtendedUploadRegistryBuilder(group)),
            optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

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
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            internalBuilder.AddWrappingManagerFor<IDownloadApi, IApizrDownloadManager, ApizrDownloadManager>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            internalBuilder.AddAliasingManagerFor<IApizrDownloadManager<IDownloadApi> , IApizrDownloadManager>();
        }

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
        AddDownloadGroup(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedDownloadRegistryBuilder> downloadRegistry,
            Action<IApizrExtendedCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => downloadRegistry.Invoke(
                new ApizrExtendedDownloadRegistryBuilder(group)),
            optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

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
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {

            // Upload
            internalBuilder.AddWrappingManagerFor<ITransferApi, IApizrUploadManager<ITransferApi>, ApizrUploadManager<ITransferApi>>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            // Download
            internalBuilder.AddWrappingManagerFor<ITransferApi, IApizrDownloadManager<ITransferApi>, ApizrDownloadManager<ITransferApi>>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            // Transfer
            internalBuilder.AddWrappingManagerFor<ITransferApi, IApizrTransferManager, ApizrTransferManager>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

            internalBuilder.AddAliasingManagerFor<IApizrTransferManager<ITransferApi>, IApizrTransferManager>();
        }

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
        AddTransferGroup(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedTransferRegistryBuilder> transferRegistry,
            Action<IApizrExtendedCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => transferRegistry.Invoke(
                new ApizrExtendedTransferRegistryBuilder(group)),
            optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

        return builder;
    }

    #endregion

    #endregion

    #endregion
}