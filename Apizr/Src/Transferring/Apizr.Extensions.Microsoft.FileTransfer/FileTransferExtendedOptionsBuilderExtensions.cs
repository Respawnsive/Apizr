using System;
using System.Collections.Generic;
using System.Net.Http;
using Apizr.Extending.Configuring.Manager;
using Apizr.Extending.Configuring.Proper;
using Apizr.Extending.Configuring.Registry;
using Apizr.Logging;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
using Microsoft.Extensions.DependencyInjection;

[assembly: Apizr.Preserve]
namespace Apizr;

/// <summary>
/// File transfer options builder extensions
/// </summary>
public static class FileTransferExtendedOptionsBuilderExtensions
{
    #region Manager

    #region Single

    #region Upload

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
        where TUploadApi : IUploadApi<HttpResponseMessage>
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
    /// Add an upload manager for the provided upload api derived from IUploadApi
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrUploadManagerFor<TUploadApi, TUploadApiResultData>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi<TUploadApiResultData> =>
        services.AddApizrManagerFor<TUploadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody))
            .TryAddSingleton<IApizrUploadManager<TUploadApi, TUploadApiResultData>, ApizrUploadManager<TUploadApi, TUploadApiResultData>>();

    /// <summary>
    /// Add an upload manager for the provided upload api derived from IUploadApi
    /// </summary>
    /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrUploadManagerWith<TUploadApiResultData>(this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
        services.AddApizrManagerFor<IUploadApi<TUploadApiResultData>>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody))
            .TryAddSingleton<IApizrUploadManagerWith<TUploadApiResultData>,
                ApizrUploadManagerWith<TUploadApiResultData>>()
            .TryAddSingleton<IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData>>(
                serviceProvider => serviceProvider.GetRequiredService<IApizrUploadManagerWith<TUploadApiResultData>>());

    #endregion

    #region Download

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
        where TDownloadApi : IDownloadApi<IDictionary<string, object>>
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
    /// Add a download manager for the provided download api derived from IDownloadApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrDownloadManagerWith<TDownloadParams>(
        this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
        services.AddApizrManagerFor<IDownloadApi<TDownloadParams>>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody))
            .TryAddSingleton<IApizrDownloadManagerWith<TDownloadParams>, ApizrDownloadManagerWith<TDownloadParams>>()
            .TryAddSingleton<IApizrDownloadManager<IDownloadApi<TDownloadParams>, TDownloadParams>>(serviceProvider =>
                serviceProvider.GetRequiredService<IApizrDownloadManagerWith<TDownloadParams>>());

    #endregion

    #region Transfer

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
        where TTransferApi : ITransferApi<IDictionary<string, object>, HttpResponseMessage>
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
        where TTransferApi : ITransferApi<TDownloadParams, HttpResponseMessage>, IUploadApi =>
        services.AddApizrManagerFor<TTransferApi>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody))
            .AddApizrDownloadManagerFor<TTransferApi, TDownloadParams>()
            .AddApizrUploadManagerFor<TTransferApi>()
            .TryAddSingleton<IApizrTransferManager<TTransferApi, TDownloadParams>,
                ApizrTransferManager<TTransferApi, TDownloadParams>>();

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrTransferManagerFor<TTransferApi, TDownloadParams, TUploadApiResultData>(
        this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi<TDownloadParams, TUploadApiResultData> =>
        services.AddApizrManagerFor<TTransferApi>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody))
            .AddApizrDownloadManagerFor<TTransferApi, TDownloadParams>()
            .AddApizrUploadManagerFor<TTransferApi, TUploadApiResultData>()
            .TryAddSingleton<IApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData>,
                ApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData>>();

    /// <summary>
    /// Add a transfer manager with shortcuts
    /// </summary>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
    /// <param name="services">The service collection where to add the manager</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IServiceCollection AddApizrTransferManagerWith<TDownloadParams, TUploadApiResultData>(
        this IServiceCollection services,
        Action<IApizrExtendedManagerOptionsBuilder> optionsBuilder = null) =>
        services.AddApizrManagerFor<ITransferApi<TDownloadParams, TUploadApiResultData>>(
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody))
            .AddApizrDownloadManagerFor<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams>()
            .AddApizrDownloadManagerWith<TDownloadParams>()
            .AddApizrUploadManagerFor<ITransferApi<TDownloadParams, TUploadApiResultData>, TUploadApiResultData>()
            .AddApizrUploadManagerWith<TUploadApiResultData>()
            .TryAddSingleton<IApizrTransferManagerWith<TDownloadParams, TUploadApiResultData>,
                ApizrTransferManagerWith<TDownloadParams, TUploadApiResultData>>()
            .TryAddSingleton<
                IApizrTransferManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams,
                    TUploadApiResultData>>(serviceProvider =>
                serviceProvider.GetRequiredService<IApizrTransferManagerWith<TDownloadParams, TUploadApiResultData>>());

    #endregion

    #endregion

    #region Multiple

    #region Upload

    /// <summary>
    /// Add an upload manager for IUploadApi
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddUploadManager(this IApizrExtendedRegistryBuilder builder,
        Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        => builder.AddUploadManagerFor<IUploadApi>(optionsBuilder);

    /// <summary>
    /// Add an upload manager for the provided upload api derived from IUploadApi
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddUploadManagerFor<TUploadApi>(
        this IApizrExtendedRegistryBuilder builder, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi<HttpResponseMessage>
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            if (typeof(TUploadApi) == typeof(IUploadApi))
            {
                internalBuilder.AddWrappingManagerFor<IUploadApi, IApizrUploadManager, ApizrUploadManager>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

                internalBuilder.AddAliasingManagerFor<IApizrUploadManager<IUploadApi>, IApizrUploadManager>();

                internalBuilder.AddAliasingManagerFor<IApizrUploadManager<IUploadApi, HttpResponseMessage>, IApizrUploadManager>();
            }
            else
                internalBuilder
                    .AddWrappingManagerFor<TUploadApi, IApizrUploadManager<TUploadApi>, ApizrUploadManager<TUploadApi>>(
                        optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));
        }

        return builder;
    }

    /// <summary>
    /// Add an upload manager for the provided upload api derived from IUploadApi
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddUploadManagerFor<TUploadApi, TUploadApiResultData>(
        this IApizrExtendedRegistryBuilder builder, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi<TUploadApiResultData>
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            internalBuilder
                .AddWrappingManagerFor<TUploadApi, IApizrUploadManager<TUploadApi, TUploadApiResultData>, ApizrUploadManager<TUploadApi, TUploadApiResultData>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));
        }

        return builder;
    }

    /// <summary>
    /// Add an upload manager with result shortcut
    /// </summary>
    /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddUploadManagerWith<TUploadApiResultData>(
        this IApizrExtendedRegistryBuilder builder, Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            internalBuilder
                .AddWrappingManagerFor<IUploadApi<TUploadApiResultData>, IApizrUploadManagerWith<TUploadApiResultData>, ApizrUploadManagerWith<TUploadApiResultData>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            internalBuilder.AddAliasingManagerFor<IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData>, IApizrUploadManagerWith<TUploadApiResultData>>();
        }

        return builder;
    }

    #endregion

    #region Download

    /// <summary>
    /// Add a download manager for IDownloadApi
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddDownloadManager(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        => builder.AddDownloadManagerFor<IDownloadApi>(optionsBuilder);

    /// <summary>
    /// Add a download manager for the provided download api derived from IDownloadApi
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddDownloadManagerFor<TDownloadApi>(this IApizrExtendedRegistryBuilder builder,
        Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi<IDictionary<string, object>>
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            if (typeof(TDownloadApi) == typeof(IDownloadApi))
            {
                internalBuilder.AddWrappingManagerFor<IDownloadApi, IApizrDownloadManager, ApizrDownloadManager>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

                internalBuilder.AddAliasingManagerFor<IApizrDownloadManager<IDownloadApi>, IApizrDownloadManager>();

                internalBuilder.AddAliasingManagerFor<IApizrDownloadManager<IDownloadApi, IDictionary<string, object>>, IApizrDownloadManager>();
            }
            else
                internalBuilder
                    .AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi>,
                        ApizrDownloadManager<TDownloadApi>>(
                        optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));
        }

        return builder;
    }

    /// <summary>
    /// Add a download manager for the provided download api derived from IDownloadApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddDownloadManagerFor<TDownloadApi, TDownloadParams>(this IApizrExtendedRegistryBuilder builder,
        Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi<TDownloadParams>
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            internalBuilder
                .AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi, TDownloadParams>,
                    ApizrDownloadManager<TDownloadApi, TDownloadParams>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));
        }

        return builder;
    }

    /// <summary>
    /// Add a download manager with parameter shortcut
    /// </summary>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddDownloadManagerWith<TDownloadParams>(this IApizrExtendedRegistryBuilder builder,
        Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            internalBuilder
                .AddWrappingManagerFor<IDownloadApi<TDownloadParams>, IApizrDownloadManagerWith<TDownloadParams>,
                    ApizrDownloadManagerWith<TDownloadParams>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            internalBuilder.AddAliasingManagerFor<IApizrDownloadManager<IDownloadApi<TDownloadParams>, TDownloadParams>, IApizrDownloadManagerWith<TDownloadParams>>();
        }

        return builder;
    }

    #endregion

    #region Transfer

    /// <summary>
    /// Add a transfer manager for ITransferApi
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddTransferManager(
            this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
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

            internalBuilder.AddAliasingManagerFor<IApizrTransferManager<ITransferApi, IDictionary<string, object>>, IApizrTransferManager>();

            internalBuilder.AddAliasingManagerFor<IApizrTransferManager<ITransferApi, IDictionary<string, object>, HttpResponseMessage>, IApizrTransferManager>();
        }

        return builder;
    }

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder AddTransferManagerFor<TTransferApi>(
        this IApizrExtendedRegistryBuilder builder,
        Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) 
        where TTransferApi : ITransferApi<IDictionary<string, object>, HttpResponseMessage>
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            // Upload
            internalBuilder
                .AddWrappingManagerFor<TTransferApi, IApizrUploadManager<TTransferApi>,
                    ApizrUploadManager<TTransferApi>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            // Download
            internalBuilder
                .AddWrappingManagerFor<TTransferApi, IApizrDownloadManager<TTransferApi>,
                    ApizrDownloadManager<TTransferApi>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            if (typeof(TTransferApi) == typeof(ITransferApi))
            {
                // Transfer
                internalBuilder
                    .AddWrappingManagerFor<ITransferApi, IApizrTransferManager,
                        ApizrTransferManager>(
                        optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody |
                                                          HttpMessageParts.ResponseBody));

                internalBuilder.AddAliasingManagerFor<IApizrTransferManager<ITransferApi>, IApizrTransferManager>();

                internalBuilder.AddAliasingManagerFor<IApizrTransferManager<ITransferApi, IDictionary<string, object>>, IApizrTransferManager>();

                internalBuilder.AddAliasingManagerFor<IApizrTransferManager<ITransferApi, IDictionary<string, object>, HttpResponseMessage>, IApizrTransferManager>();
            }
            else
            {
                // Transfer
                internalBuilder
                    .AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi>,
                        ApizrTransferManager<TTransferApi>>(
                        optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody |
                                                          HttpMessageParts.ResponseBody));
            }
        }

        return builder;
    }

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where TTransferApi : ITransferApi<TDownloadParams, HttpResponseMessage>, IUploadApi
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            // Upload
            internalBuilder
                .AddWrappingManagerFor<TTransferApi, IApizrUploadManager<TTransferApi>,
                    ApizrUploadManager<TTransferApi>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            // Download
            internalBuilder
                .AddWrappingManagerFor<TTransferApi, IApizrDownloadManager<TTransferApi, TDownloadParams>,
                    ApizrDownloadManager<TTransferApi, TDownloadParams>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            // Transfer
            internalBuilder
                .AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi, TDownloadParams>,
                    ApizrTransferManager<TTransferApi, TDownloadParams>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));
        }

        return builder;
    }

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddTransferManagerFor<TTransferApi, TDownloadParams, TUploadApiResultData>(this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null) where TTransferApi : ITransferApi<TDownloadParams, TUploadApiResultData>
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            // Upload
            internalBuilder
                .AddWrappingManagerFor<TTransferApi, IApizrUploadManager<TTransferApi, TUploadApiResultData>,
                    ApizrUploadManager<TTransferApi, TUploadApiResultData>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            // Download
            internalBuilder
                .AddWrappingManagerFor<TTransferApi, IApizrDownloadManager<TTransferApi, TDownloadParams>,
                    ApizrDownloadManager<TTransferApi, TDownloadParams>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            // Transfer
            internalBuilder
                .AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData>,
                    ApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));
        }

        return builder;
    }

    /// <summary>
    /// Add a transfer manager for the provided transfer api derived from ITransferApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrExtendedRegistryBuilder
        AddTransferManagerWith<TDownloadParams, TUploadApiResultData>(this IApizrExtendedRegistryBuilder builder,
            Action<IApizrExtendedProperOptionsBuilder> optionsBuilder = null)
    {
        if (builder is IApizrInternalExtendedRegistryBuilder<IApizrExtendedProperOptionsBuilder> internalBuilder)
        {
            // Upload
            internalBuilder
                .AddWrappingManagerFor<ITransferApi<TDownloadParams, TUploadApiResultData>, IApizrUploadManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TUploadApiResultData>,
                    ApizrUploadManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TUploadApiResultData>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            builder.AddUploadManagerWith<TUploadApiResultData>(optionsBuilder);

            // Download
            internalBuilder
                .AddWrappingManagerFor<ITransferApi<TDownloadParams, TUploadApiResultData>, IApizrDownloadManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams>,
                    ApizrDownloadManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            builder.AddDownloadManagerWith<TDownloadParams>(optionsBuilder);

            // Transfer
            internalBuilder
                .AddWrappingManagerFor<ITransferApi<TDownloadParams, TUploadApiResultData>, IApizrTransferManagerWith<TDownloadParams, TUploadApiResultData>,
                    ApizrTransferManagerWith<TDownloadParams, TUploadApiResultData>>(
                    optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

            internalBuilder.AddAliasingManagerFor<IApizrTransferManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams, TUploadApiResultData>, IApizrTransferManagerWith<TDownloadParams, TUploadApiResultData>>();
        }

        return builder;
    }

    #endregion

    #endregion

    #endregion
}