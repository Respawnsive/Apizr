using System;
using Apizr.Configuring.Common;
using Apizr.Configuring.Manager;
using Apizr.Configuring.Proper;
using Apizr.Configuring.Registry;
using Apizr.Configuring.Request;
using Apizr.Configuring.Shared;
using Apizr.Logging;
using Apizr.Progressing;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;

namespace Apizr;

/// <summary>
/// File transfer options builder extensions
/// </summary>
public static class FileTransferOptionsBuilderExtensions
{
    #region Transfer

    #region Single

    /// <summary>
    /// Create an upload manager for IUploadApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrUploadManager CreateUploadManager(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder) =>
        new ApizrUploadManager(
            builder.CreateManagerFor<IUploadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody)));

    /// <summary>
    /// Create an upload manager for the provided upload api derived from IUploadApi
    /// </summary>
    /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrUploadManager<TUploadApi> CreateUploadManagerFor<TUploadApi>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TUploadApi : IUploadApi =>
        new ApizrUploadManager<TUploadApi>(
            builder.CreateManagerFor<TUploadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody)));

    /// <summary>
    /// Create a download manager for IDownloadApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrDownloadManager CreateDownloadManager(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null) =>
        new ApizrDownloadManager(
            builder.CreateManagerFor<IDownloadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody)));

    /// <summary>
    /// Create a download manager for the provided download api derived from IDownloadApi
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrDownloadManager<TDownloadApi> CreateDownloadManagerFor<TDownloadApi>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi =>
        new ApizrDownloadManager<TDownloadApi>(
            builder.CreateManagerFor<TDownloadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody)));

    /// <summary>
    /// Create a download manager for the provided download api derived from IDownloadApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrDownloadManager<TDownloadApi, TDownloadParams> CreateDownloadManagerFor<TDownloadApi,
        TDownloadParams>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TDownloadApi : IDownloadApi<TDownloadParams> =>
        new ApizrDownloadManager<TDownloadApi, TDownloadParams>(
            builder.CreateManagerFor<TDownloadApi>(optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody)));

    /// <summary>
    /// Create a transfer manager for ITransferApi (you must at least provide a base url thanks to the options builder)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrTransferManager CreateTransferManager(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null) =>
        new ApizrTransferManager(
            builder.CreateDownloadManagerFor<ITransferApi>(IgnoreMessageParts(optionsBuilder,
                HttpMessageParts.ResponseBody)),
            builder.CreateUploadManagerFor<ITransferApi>(IgnoreMessageParts(optionsBuilder,
                HttpMessageParts.RequestBody)));

    /// <summary>
    /// Create a transfer manager for the provided transfer api derived from ITransferApi
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrTransferManager<TTransferApi> CreateTransferManagerFor<TTransferApi>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi =>
        new ApizrTransferManager<TTransferApi>(
            builder.CreateDownloadManagerFor<TTransferApi>(IgnoreMessageParts(optionsBuilder,
                HttpMessageParts.ResponseBody)),
            builder.CreateUploadManagerFor<TTransferApi>(IgnoreMessageParts(optionsBuilder,
                HttpMessageParts.RequestBody)));

    /// <summary>
    /// Create a transfer manager for the provided transfer api derived from ITransferApi{TDownloadParams}
    /// </summary>
    /// <typeparam name="TTransferApi">The transfer api interface to manage</typeparam>
    /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrTransferManager<TTransferApi, TDownloadParams> CreateTransferManagerFor<TTransferApi,
        TDownloadParams>(this IApizrBuilder builder,
        Action<IApizrManagerOptionsBuilder> optionsBuilder = null)
        where TTransferApi : ITransferApi<TDownloadParams> =>
        new ApizrTransferManager<TTransferApi, TDownloadParams>(
            builder.CreateDownloadManagerFor<TTransferApi, TDownloadParams>(IgnoreMessageParts(optionsBuilder,
                HttpMessageParts.ResponseBody)),
            builder.CreateUploadManagerFor<TTransferApi>(IgnoreMessageParts(optionsBuilder,
                HttpMessageParts.RequestBody)));

    #endregion
    
    #region Multiple

    #region Add

    /// <summary>
    /// Add an upload manager for IUploadApi (you must at least provide a base url)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrRegistryBuilder AddUploadManager(this IApizrRegistryBuilder builder,
        Action<IApizrProperOptionsBuilder> optionsBuilder)
    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrProperOptionsBuilder> internalBuilder)
        {
            internalBuilder.AddWrappingManagerFor<IUploadApi, IApizrUploadManager>(
                apizrManager => new ApizrUploadManager(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody));

            internalBuilder.AddAliasingManagerFor<IApizrUploadManager, IApizrUploadManager<IUploadApi>>();
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
    public static IApizrRegistryBuilder
        AddUploadManager(
            this IApizrRegistryBuilder builder,
            Action<IApizrUploadRegistryBuilder> uploadRegistry,
            Action<IApizrCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => uploadRegistry.Invoke(
                new ApizrUploadRegistryBuilder(group)), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Add a download manager for IDownloadApi (you must at least provide a base url)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrRegistryBuilder
        AddDownloadManager(
            this IApizrRegistryBuilder builder,
            Action<IApizrProperOptionsBuilder> optionsBuilder)
    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrProperOptionsBuilder> internalBuilder)
        {
            internalBuilder.AddWrappingManagerFor<IDownloadApi, IApizrDownloadManager>(
                apizrManager => new ApizrDownloadManager(apizrManager),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.ResponseBody));

            internalBuilder.AddAliasingManagerFor<IApizrDownloadManager, IApizrDownloadManager<IDownloadApi>>();
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
    public static IApizrRegistryBuilder
        AddDownloadManager(
            this IApizrRegistryBuilder builder,
            Action<IApizrDownloadRegistryBuilder> downloadRegistry,
            Action<IApizrCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => downloadRegistry.Invoke(
                new ApizrDownloadRegistryBuilder(group)), optionsBuilder);

        return builder;
    }

    /// <summary>
    /// Add a transfer manager for ITransferApi (you must at least provide a base url)
    /// </summary>
    /// <param name="builder">The builder to create the manager from</param>
    /// <param name="optionsBuilder">The builder defining some options</param>
    /// <returns></returns>
    public static IApizrRegistryBuilder
        AddTransferManager(
            this IApizrRegistryBuilder builder,
            Action<IApizrProperOptionsBuilder> optionsBuilder)
    {
        if (builder is IApizrInternalRegistryBuilderBase<IApizrProperOptionsBuilder> internalBuilder)
        {
            internalBuilder.AddWrappingManagerFor<ITransferApi, IApizrTransferManager>(apizrManager =>
                    new ApizrTransferManager(new ApizrDownloadManager<ITransferApi>(apizrManager),
                        new ApizrUploadManager<ITransferApi>(apizrManager)),
                optionsBuilder.IgnoreMessageParts(HttpMessageParts.RequestBody | HttpMessageParts.ResponseBody));

            internalBuilder.AddAliasingManagerFor<IApizrTransferManager, IApizrTransferManager<ITransferApi>>();
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
    public static IApizrRegistryBuilder
        AddTransferManager(
            this IApizrRegistryBuilder builder,
            Action<IApizrTransferRegistryBuilder> transferRegistry,
            Action<IApizrCommonOptionsBuilder> optionsBuilder = null)
    {
        builder.AddGroup(
            group => transferRegistry.Invoke(
                new ApizrTransferRegistryBuilder(group)), optionsBuilder);

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
    /// Check if registry contains a manager for the default <typeparamref name="IUploadApi"/> api type
    /// </summary>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool ContainsUploadManager(
        this IApizrEnumerableRegistry registry) =>
        ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrUploadManager<IUploadApi>>();

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
    /// Check if registry contains a manager for the default <typeparamref name="IDownloadApi"/> api type
    /// </summary>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool ContainsDownloadManager(
        this IApizrEnumerableRegistry registry) =>
        ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrDownloadManager<IDownloadApi>>();

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
    /// Check if registry contains a manager for the default <typeparamref name="ITransferApi"/> api type
    /// </summary>
    /// <param name="registry">The registry to get the manager from</param>
    /// <returns></returns>
    public static bool ContainsTransferManager(
        this IApizrEnumerableRegistry registry) =>
        ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrTransferManager<ITransferApi>>();

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
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static TBuilder WithProgress<TBuilder>(this TBuilder builder)
        where TBuilder : IApizrGlobalSharedOptionsBuilderBase
    {
        if (builder is IApizrInternalRegistrationOptionsBuilder registrationBuilder)
            registrationBuilder.AddDelegatingHandler(_ =>
                new ApizrProgressHandler());

        return builder;
    }

    /// <summary>
    /// Tells Apizr to report any transfer progress
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <param name="onProgress">The action called back on any progress</param>
    /// <returns></returns>
    public static TBuilder WithProgress<TBuilder>(this TBuilder builder, Action<ApizrProgressEventArgs> onProgress)
        where TBuilder : IApizrGlobalSharedOptionsBuilderBase
        => builder.WithProgress(new ApizrProgress(onProgress));

    /// <summary>
    /// Tells Apizr to report any transfer progress
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    /// <param name="builder"></param>
    /// <param name="progress">The progress reporter</param>
    /// <returns></returns>
    public static TBuilder WithProgress<TBuilder>(this TBuilder builder, IApizrProgress progress)
        where TBuilder : IApizrGlobalSharedOptionsBuilderBase
    {
        if (builder is IApizrInternalRegistrationOptionsBuilder registrationBuilder)
            registrationBuilder.AddDelegatingHandler(_ =>
                new ApizrProgressHandler());

        if (builder is IApizrInternalOptionsBuilder voidBuilder)
            voidBuilder.SetHandlerParameter(Constants.ApizrProgressKey, progress);

        return builder;
    }

    #endregion

    #region Path

    /// <summary>
    /// Tells Apizr to set the ending of the request uri with the provided path
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="builder"></param>
    /// <param name="dynamicPath">The path ending the request uri</param>
    /// <returns></returns>
    public static T WithDynamicPath<T>(this T builder, string dynamicPath)
        where T : IApizrRequestOptionsBuilderBase
    {
        if (builder is IApizrInternalOptionsBuilder voidBuilder)
            voidBuilder.SetHandlerParameter(Constants.ApizrDynamicPathKey, dynamicPath);

        return builder;
    }

    #endregion

    #region Internal

    internal static Action<T> IgnoreMessageParts<T>(this 
        Action<T> optionsBuilder, HttpMessageParts messageParts)
    where T : IApizrGlobalSharedOptionsBuilderBase
    {
        if (optionsBuilder == null)
            optionsBuilder = optionsBuilderInstance =>
                ((IApizrInternalOptionsBuilder)optionsBuilderInstance).SetHandlerParameter(Constants.ApizrIgnoreMessagePartsKey, messageParts);
        else
            optionsBuilder += optionsBuilderInstance =>
                ((IApizrInternalOptionsBuilder)optionsBuilderInstance).SetHandlerParameter(Constants.ApizrIgnoreMessagePartsKey, messageParts);

        return optionsBuilder;
    }

    #endregion
}