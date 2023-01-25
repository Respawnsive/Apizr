using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Registry;
using Apizr.Configuring.Shared;
using Apizr.Progressing;
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

    #region ApizrBuilder

    /// <summary>
    /// Create an <see cref="ApizrUploadManager{TUploadApi}"/> instance
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrUploadManager<TUploadApi> CreateUploadManagerFor<TUploadApi>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi =>
        new ApizrUploadManager<TUploadApi>(builder.CreateManagerFor<TUploadApi>(optionsBuilder));

    /// <summary>
    /// Create an <see cref="ApizrDownloadManager{TUploadApi}"/> instance
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrDownloadManager<TDownloadApi> CreateDownloadManagerFor<TDownloadApi>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi =>
        new ApizrDownloadManager<TDownloadApi>(builder.CreateManagerFor<TDownloadApi>(optionsBuilder));

    /// <summary>
    /// Create an <see cref="ApizrDownloadManager{TUploadApi, TDownloadParams}"/> instance
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrDownloadManager<TDownloadApi, TDownloadParams> CreateDownloadManagerFor<TDownloadApi, TDownloadParams>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi<TDownloadParams> =>
        new ApizrDownloadManager<TDownloadApi, TDownloadParams>(builder.CreateManagerFor<TDownloadApi>(optionsBuilder));

    /// <summary>
    /// Create an <see cref="ApizrTransferManager{TTransferApi}"/> instance
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrTransferManager<TTransferApi> CreateTransferManagerFor<TTransferApi>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi =>
        new ApizrTransferManager<TTransferApi>(builder.CreateDownloadManagerFor<TTransferApi>(optionsBuilder),
            builder.CreateUploadManagerFor<TTransferApi>(optionsBuilder));

    /// <summary>
    /// Create an <see cref="ApizrTransferManager{TTransferApi, TDownloadParams}"/> instance
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrTransferManager<TTransferApi, TDownloadParams> CreateTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi<TDownloadParams> =>
        new ApizrTransferManager<TTransferApi, TDownloadParams>(builder.CreateDownloadManagerFor<TTransferApi, TDownloadParams>(optionsBuilder),
            builder.CreateUploadManagerFor<TTransferApi>(optionsBuilder));

    #endregion

    #region ApizrRegistry

    #region Add

    /// <summary>
    /// Create an <see cref="ApizrUploadManager{TUploadApi}"/> instance
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="uploadRegistry">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static TApizrRegistryBuilder
        AddUploadManager<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>(
            this IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder> builder,
            Action<IApizrUploadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>> uploadRegistry,
            Action<TApizrCommonOptionsBuilder> optionsBuilder = null)
        where TApizrRegistry : IApizrEnumerableRegistry
        where TApizrRegistryBuilder : IApizrRegistryBuilderBase<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder, TApizrCommonOptionsBuilder>
        where TApizrProperOptionsBuilder : IApizrGlobalProperOptionsBuilderBase
        where TApizrCommonOptionsBuilder : IApizrGlobalCommonOptionsBuilderBase
    {
        builder.AddGroup(
            group => uploadRegistry.Invoke(
                new ApizrUploadRegistryBuilder<TApizrRegistry, TApizrRegistryBuilder, TApizrProperOptionsBuilder,
                    TApizrCommonOptionsBuilder>(group)), optionsBuilder);

        return (TApizrRegistryBuilder) builder;
    }

    /// <summary>
    /// Create an <see cref="ApizrUploadManager{TUploadApi}"/> instance
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrRegistryBuilder AddUploadManagerFor<TUploadApi>(this IApizrRegistryBuilder builder, Action<IApizrProperOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi

    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrProperOptionsBuilder> internalBuilder)
            internalBuilder.AddWrappingManagerFor<TUploadApi, IApizrUploadManager<TUploadApi>>(apizrManager => new ApizrUploadManager<TUploadApi>(apizrManager), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Create an <see cref="ApizrDownloadManager{TDownloadApi}"/> instance
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrRegistryBuilder AddDownloadManagerFor<TDownloadApi>(this IApizrRegistryBuilder builder, Action<IApizrProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi

    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrProperOptionsBuilder> internalBuilder)
            internalBuilder.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi>>(apizrManager => new ApizrDownloadManager<TDownloadApi>(apizrManager), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Create an <see cref="ApizrDownloadManager{TDownloadApi, TDownloadParams}"/> instance
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrRegistryBuilder AddDownloadManagerFor<TDownloadApi, TDownloadParams>(this IApizrRegistryBuilder builder, Action<IApizrProperOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi<TDownloadParams>

    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrProperOptionsBuilder> internalBuilder)
            internalBuilder.AddWrappingManagerFor<TDownloadApi, IApizrDownloadManager<TDownloadApi, TDownloadParams>>(apizrManager => new ApizrDownloadManager<TDownloadApi, TDownloadParams>(apizrManager), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Create an <see cref="ApizrTransferManager{TTransferApi}"/> instance
    /// </summary>
    /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrRegistryBuilder AddTransferManagerFor<TTransferApi>(this IApizrRegistryBuilder builder, Action<IApizrProperOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi

    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrProperOptionsBuilder> internalBuilder)
            internalBuilder.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi>>(
                apizrManager => new ApizrTransferManager<TTransferApi>(
                    new ApizrDownloadManager<TTransferApi>(apizrManager),
                    new ApizrUploadManager<TTransferApi>(apizrManager)), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Create an <see cref="ApizrTransferManager{TTransferApi, TDownloadParams}"/> instance
    /// </summary>
    /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrRegistryBuilder AddTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrRegistryBuilder builder, Action<IApizrProperOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi<TDownloadParams>

    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrProperOptionsBuilder> internalBuilder)
            internalBuilder.AddWrappingManagerFor<TTransferApi, IApizrTransferManager<TTransferApi, TDownloadParams>>(
                apizrManager => new ApizrTransferManager<TTransferApi, TDownloadParams>(
                    new ApizrDownloadManager<TTransferApi, TDownloadParams>(apizrManager),
                    new ApizrUploadManager<TTransferApi>(apizrManager)), optionsBuilder);

        return builder;
    }

    #endregion

    #region Get

    /// <summary>
    /// Get an upload manager instance
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static IApizrUploadManager<TUploadApi> GetUploadManagerFor<TUploadApi>(
        this IApizrEnumerableRegistry registry)
        where TUploadApi : IUploadApi =>
        ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrUploadManager<TUploadApi>>();

    /// <summary>
    /// Get a download manager instance
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static IApizrDownloadManager<TDownloadApi> GetDownloadManagerFor<TDownloadApi>(this IApizrEnumerableRegistry registry)
        where TDownloadApi : IDownloadApi =>
        ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrDownloadManager<TDownloadApi>>();

    /// <summary>
    /// Get a download manager instance
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static IApizrDownloadManager<TDownloadApi, TDownloadParams> GetDownloadManagerFor<TDownloadApi, TDownloadParams>(this IApizrEnumerableRegistry registry)
        where TDownloadApi : IDownloadApi<TDownloadParams> =>
        ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrDownloadManager<TDownloadApi, TDownloadParams>>();

    /// <summary>
    /// Get a transfer manager instance
    /// </summary>
    /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static IApizrTransferManager<TTransferApi> GetTransferManagerFor<TTransferApi>(this IApizrEnumerableRegistry registry)
        where TTransferApi : ITransferApi =>
        ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrTransferManager<TTransferApi>>();

    /// <summary>
    /// Get a transfer manager instance
    /// </summary>
    /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static IApizrTransferManager<TTransferApi, TDownloadParams> GetTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrEnumerableRegistry registry)
        where TTransferApi : ITransferApi<TDownloadParams> =>
        ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams>>();


    #endregion

    #region TryGet

    /// <summary>
    /// Get an upload manager instance
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool TryGetUploadManagerFor<TUploadApi>(
        this IApizrEnumerableRegistry registry, out IApizrUploadManager<TUploadApi> manager)
        where TUploadApi : IUploadApi =>
        ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrUploadManager<TUploadApi>>(out manager);

    /// <summary>
    /// Get a download manager instance
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool TryGetDownloadManagerFor<TDownloadApi>(this IApizrEnumerableRegistry registry, out IApizrDownloadManager<TDownloadApi> manager)
        where TDownloadApi : IDownloadApi =>
        ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrDownloadManager<TDownloadApi>>(out manager);

    /// <summary>
    /// Get a download manager instance
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool TryGetDownloadManagerFor<TDownloadApi, TDownloadParams>(this IApizrEnumerableRegistry registry, out IApizrDownloadManager<TDownloadApi, TDownloadParams> manager)
        where TDownloadApi : IDownloadApi<TDownloadParams> =>
        ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrDownloadManager<TDownloadApi, TDownloadParams>>(out manager);

    /// <summary>
    /// Get a transfer manager instance
    /// </summary>
    /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool TryGetTransferManagerFor<TTransferApi>(this IApizrEnumerableRegistry registry, out IApizrTransferManager<TTransferApi> manager)
        where TTransferApi : ITransferApi =>
        ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrTransferManager<TTransferApi>>(out manager);

    /// <summary>
    /// Get a transfer manager instance
    /// </summary>
    /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool TryGetTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrEnumerableRegistry registry, out IApizrTransferManager<TTransferApi, TDownloadParams> manager)
        where TTransferApi : ITransferApi<TDownloadParams> =>
        ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams>>(out manager);


    #endregion

    #region Contains

    /// <summary>
    /// Check if registry contains a manager for <typeparamref name="TUploadApi"/> api type
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool ContainsUploadManagerFor<TUploadApi>(
        this IApizrEnumerableRegistry registry)
        where TUploadApi : IUploadApi =>
        ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrUploadManager<TUploadApi>>();

    /// <summary>
    /// Check if registry contains a manager for <typeparamref name="TDownloadApi"/> api type
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool ContainsDownloadManagerFor<TDownloadApi>(this IApizrEnumerableRegistry registry)
        where TDownloadApi : IDownloadApi =>
        ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrDownloadManager<TDownloadApi>>();

    /// <summary>
    /// Check if registry contains a manager for <typeparamref name="TDownloadApi"/> api type
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool ContainsDownloadManagerFor<TDownloadApi, TDownloadParams>(this IApizrEnumerableRegistry registry)
        where TDownloadApi : IDownloadApi<TDownloadParams> =>
        ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrDownloadManager<TDownloadApi, TDownloadParams>>();

    /// <summary>
    /// Check if registry contains a manager for <typeparamref name="TTransferApi"/> api type
    /// </summary>
    /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool ContainsTransferManagerFor<TTransferApi>(this IApizrEnumerableRegistry registry)
        where TTransferApi : ITransferApi =>
        ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrTransferManager<TTransferApi>>();

    /// <summary>
    /// Check if registry contains a manager for <typeparamref name="TTransferApi"/> api type
    /// </summary>
    /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool ContainsTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrEnumerableRegistry registry)
        where TTransferApi : ITransferApi<TDownloadParams> =>
        ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams>>();


    #endregion

    #endregion

    #endregion

    #region Progress

    /// <summary>
    /// Enables transfer progress reporting with Apizr
    /// (you should provide a progress callback or reporter at request time)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static T WithProgress<T>(this T builder)
        where T : IApizrGlobalSharedOptionsBuilderBase
    {
        if (builder is IApizrInternalRegistrationOptionsBuilder registrationBuilder)
            registrationBuilder.AddDelegatingHandler(_ =>
                new ApizrProgressHandler());

        return builder;
    }

    /// <summary>
    /// Tells Apizr to report any transfer progress
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <param name="onProgress">The action called back on any progress</param>
    /// <returns></returns>
    public static T WithProgress<T>(this T builder, Action<ApizrProgressEventArgs> onProgress)
        where T : IApizrGlobalSharedOptionsBuilderBase
        => builder.WithProgress(new ApizrProgress(onProgress));

    /// <summary>
    /// Tells Apizr to report any transfer progress
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <param name="progress">The progress reporter</param>
    /// <returns></returns>
    public static T WithProgress<T>(this T builder, IApizrProgress progress)
        where T : IApizrGlobalSharedOptionsBuilderBase
    {
        if (builder is IApizrInternalRegistrationOptionsBuilder registrationBuilder)
            registrationBuilder.AddDelegatingHandler(_ =>
                new ApizrProgressHandler());

        if (builder is IApizrInternalOptionsBuilder voidBuilder)
            voidBuilder.SetHandlerParameter(Constants.ApizrProgressKey, progress);

        return builder;
    } 

    #endregion
}