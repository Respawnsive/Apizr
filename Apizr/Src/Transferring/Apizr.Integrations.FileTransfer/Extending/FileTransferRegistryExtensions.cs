using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Configuring.Registry;
using Apizr.Configuring.Request;
using Apizr.Transferring.Managing;
using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Extending
{
    public static class FileTransferRegistryExtensions
    {
        #region Get

        #region Upload

        /// <summary>
        /// Get an upload manager instance
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrUploadManager GetUploadManager(
            this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrUploadManager>();

        /// <summary>
        /// Get an upload manager instance
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrUploadManager<TUploadApi> GetUploadManagerFor<TUploadApi>(
            this IApizrEnumerableRegistry registry)
            where TUploadApi : IUploadApi<HttpResponseMessage> =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrUploadManager<TUploadApi>>();

        /// <summary>
        /// Get an upload manager instance
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrUploadManager<TUploadApi, TUploadApiResultData> GetUploadManagerFor<TUploadApi, TUploadApiResultData>(
            this IApizrEnumerableRegistry registry)
            where TUploadApi : IUploadApi<TUploadApiResultData> =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrUploadManager<TUploadApi, TUploadApiResultData>>();

        /// <summary>
        /// Get an upload manager instance
        /// </summary>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData> GetUploadManagerWith<TUploadApiResultData>(
            this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData>>();

        #endregion

        #region Download

        /// <summary>
        /// Get a download manager instance
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrDownloadManager GetDownloadManager(this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrDownloadManager>();

        /// <summary>
        /// Get a download manager instance
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrDownloadManager<TDownloadApi> GetDownloadManagerFor<TDownloadApi>(this IApizrEnumerableRegistry registry)
            where TDownloadApi : IDownloadApi<IDictionary<string, object>> =>
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
        /// Get a download manager instance
        /// </summary>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrDownloadManager<IDownloadApi<TDownloadParams>, TDownloadParams> GetDownloadManagerWith<TDownloadParams>(this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrDownloadManager<IDownloadApi<TDownloadParams>, TDownloadParams>>();

        #endregion

        #region Transfer

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrTransferManager GetTransferManager(this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrTransferManager>();

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrTransferManager<TTransferApi> GetTransferManagerFor<TTransferApi>(this IApizrEnumerableRegistry registry)
            where TTransferApi : ITransferApi<IDictionary<string, object>, HttpResponseMessage> =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrTransferManager<TTransferApi>>();

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrTransferManager<TTransferApi, TDownloadParams> GetTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrEnumerableRegistry registry)
            where TTransferApi : ITransferApi<TDownloadParams, HttpResponseMessage>, IUploadApi =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams>>();

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData> GetTransferManagerFor<TTransferApi, TDownloadParams, TUploadApiResultData>(this IApizrEnumerableRegistry registry)
            where TTransferApi : ITransferApi<TDownloadParams, TUploadApiResultData> =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData>>();

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static IApizrTransferManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams, TUploadApiResultData> GetTransferManagerWith<TDownloadParams, TUploadApiResultData>(this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).GetManagerInternal<IApizrTransferManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams, TUploadApiResultData>>();

        #endregion

        #endregion

        #region TryGet

        #region Upload

        /// <summary>
        /// Get an upload manager instance
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The upload manager instance</param>
        /// <returns></returns>
        public static bool TryGetUploadManager(
            this IApizrEnumerableRegistry registry, out IApizrUploadManager manager) =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrUploadManager>(out manager);

        /// <summary>
        /// Get an upload manager instance
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The upload manager instance</param>
        /// <returns></returns>
        public static bool TryGetUploadManagerFor<TUploadApi>(
            this IApizrEnumerableRegistry registry, out IApizrUploadManager<TUploadApi> manager)
            where TUploadApi : IUploadApi<HttpResponseMessage> =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrUploadManager<TUploadApi>>(out manager);

        /// <summary>
        /// Get an upload manager instance
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The upload manager instance</param>
        /// <returns></returns>
        public static bool TryGetUploadManagerFor<TUploadApi, TUploadApiResultData>(
            this IApizrEnumerableRegistry registry, out IApizrUploadManager<TUploadApi, TUploadApiResultData> manager)
            where TUploadApi : IUploadApi<TUploadApiResultData> =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrUploadManager<TUploadApi, TUploadApiResultData>>(out manager);

        /// <summary>
        /// Get an upload manager instance
        /// </summary>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The upload manager instance</param>
        /// <returns></returns>
        public static bool TryGetUploadManagerWith<TUploadApiResultData>(
            this IApizrEnumerableRegistry registry, out IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData> manager) =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData>>(out manager);

        #endregion

        #region Download

        /// <summary>
        /// Get a download manager instance
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The download manager instance</param>
        /// <returns></returns>
        public static bool TryGetDownloadManager(this IApizrEnumerableRegistry registry, out IApizrDownloadManager manager) =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrDownloadManager>(out manager);

        /// <summary>
        /// Get a download manager instance
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The download manager instance</param>
        /// <returns></returns>
        public static bool TryGetDownloadManagerFor<TDownloadApi>(this IApizrEnumerableRegistry registry, out IApizrDownloadManager<TDownloadApi> manager)
            where TDownloadApi : IDownloadApi<IDictionary<string, object>> =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrDownloadManager<TDownloadApi>>(out manager);

        /// <summary>
        /// Get a download manager instance
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The download manager instance</param>
        /// <returns></returns>
        public static bool TryGetDownloadManagerFor<TDownloadApi, TDownloadParams>(this IApizrEnumerableRegistry registry, out IApizrDownloadManager<TDownloadApi, TDownloadParams> manager)
            where TDownloadApi : IDownloadApi<TDownloadParams> =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrDownloadManager<TDownloadApi, TDownloadParams>>(out manager);

        /// <summary>
        /// Get a download manager instance
        /// </summary>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The download manager instance</param>
        /// <returns></returns>
        public static bool TryGetDownloadManagerWith<TDownloadParams>(this IApizrEnumerableRegistry registry, out IApizrDownloadManager<IDownloadApi<TDownloadParams>, TDownloadParams> manager) =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrDownloadManager<IDownloadApi<TDownloadParams>, TDownloadParams>>(out manager);

        #endregion

        #region Transfer

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The transfer manager instance</param>
        /// <returns></returns>
        public static bool TryGetTransferManager(this IApizrEnumerableRegistry registry, out IApizrTransferManager manager) =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrTransferManager>(out manager);

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The transfer manager instance</param>
        /// <returns></returns>
        public static bool TryGetTransferManagerFor<TTransferApi>(this IApizrEnumerableRegistry registry, out IApizrTransferManager<TTransferApi> manager)
            where TTransferApi : ITransferApi<IDictionary<string, object>, HttpResponseMessage> =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrTransferManager<TTransferApi>>(out manager);

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The transfer manager instance</param>
        /// <returns></returns>
        public static bool TryGetTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrEnumerableRegistry registry, out IApizrTransferManager<TTransferApi, TDownloadParams> manager)
            where TTransferApi : ITransferApi<TDownloadParams, HttpResponseMessage>, IUploadApi =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams>>(out manager);

        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The transfer manager instance</param>
        /// <returns></returns>
        public static bool TryGetTransferManagerFor<TTransferApi, TDownloadParams, TUploadApiResultData>(this IApizrEnumerableRegistry registry, out IApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData> manager)
            where TTransferApi : ITransferApi<TDownloadParams, TUploadApiResultData> =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData>>(out manager);


        /// <summary>
        /// Get a transfer manager instance
        /// </summary>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <param name="manager">The transfer manager instance</param>
        /// <returns></returns>
        public static bool TryGetTransferManagerWith<TDownloadParams, TUploadApiResultData>(this IApizrEnumerableRegistry registry, out IApizrTransferManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams, TUploadApiResultData> manager) =>
            ((IApizrInternalEnumerableRegistry)registry).TryGetManagerInternal<IApizrTransferManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams, TUploadApiResultData>>(out manager);

        #endregion

        #endregion

        #region Contains

        #region Upload

        /// <summary>
        /// Check if registry contains a manager for the default <see cref="IUploadApi"/> api type
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsUploadManager(
            this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrUploadManager>();

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="TUploadApi"/> api type
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsUploadManagerFor<TUploadApi>(
            this IApizrEnumerableRegistry registry)
            where TUploadApi : IUploadApi<HttpResponseMessage> =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrUploadManager<TUploadApi>>();

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="TUploadApi"/> api type
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api interface to manage</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsUploadManagerFor<TUploadApi, TUploadApiResultData>(
            this IApizrEnumerableRegistry registry)
            where TUploadApi : IUploadApi<TUploadApiResultData> =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrUploadManager<TUploadApi, TUploadApiResultData>>();

        /// <summary>
        /// Check if registry contains a manager for <see cref="IUploadApi{TUploadApiResultData}"/> api type
        /// </summary>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsUploadManagerWith<TUploadApiResultData>(
            this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrUploadManager<IUploadApi<TUploadApiResultData>, TUploadApiResultData>>();

        #endregion

        #region Download

        /// <summary>
        /// Check if registry contains a manager for the default <see cref="IDownloadApi"/> api type
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsDownloadManager(
            this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrDownloadManager>();

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="TDownloadApi"/> api type
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsDownloadManagerFor<TDownloadApi>(this IApizrEnumerableRegistry registry)
            where TDownloadApi : IDownloadApi<IDictionary<string, object>> =>
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
        /// Check if registry contains a manager for <see cref="IDownloadApi{TDownloadParams}"/> api type
        /// </summary>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsDownloadManagerWith<TDownloadParams>(this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrDownloadManager<IDownloadApi<TDownloadParams>, TDownloadParams>>();

        #endregion

        #region Transfer

        /// <summary>
        /// Check if registry contains a manager for the default <see cref="ITransferApi"/> api type
        /// </summary>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsTransferManager(
            this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrTransferManager>();

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="TTransferApi"/> api type
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsTransferManagerFor<TTransferApi>(this IApizrEnumerableRegistry registry)
            where TTransferApi : ITransferApi<IDictionary<string, object>, HttpResponseMessage> =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrTransferManager<TTransferApi>>();

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="TTransferApi"/> api type
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsTransferManagerFor<TTransferApi, TDownloadParams>(this IApizrEnumerableRegistry registry)
            where TTransferApi : ITransferApi<TDownloadParams, HttpResponseMessage>, IUploadApi =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams>>();

        /// <summary>
        /// Check if registry contains a manager for <typeparamref name="TTransferApi"/> api type
        /// </summary>
        /// <typeparam name="TTransferApi">The Transfer api interface to manage</typeparam>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsTransferManagerFor<TTransferApi, TDownloadParams, TUploadApiResultData>(this IApizrEnumerableRegistry registry)
            where TTransferApi : ITransferApi<TDownloadParams, TUploadApiResultData> =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrTransferManager<TTransferApi, TDownloadParams, TUploadApiResultData>>();

        /// <summary>
        /// Check if registry contains a manager for <see cref="ITransferApi{TDownloadParams, TUploadApiResultData}"/> api type
        /// </summary>
        /// <typeparam name="TDownloadParams">The download query parameters type</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry to get the manager from</param>
        /// <returns></returns>
        public static bool ContainsTransferManagerWith<TDownloadParams, TUploadApiResultData>(this IApizrEnumerableRegistry registry) =>
            ((IApizrInternalEnumerableRegistry)registry).ContainsManagerInternal<IApizrTransferManager<ITransferApi<TDownloadParams, TUploadApiResultData>, TDownloadParams, TUploadApiResultData>>(); 
        
        #endregion
        
        #endregion

        #region Upload

        /// <summary>
        /// Upload a file from its bytes data
        /// </summary>
        /// <param name="registry">The registry</param>
        /// <param name="byteArrayPart">The file bytes data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> UploadAsync(
            this IApizrEnumerableRegistry registry, ByteArrayPart byteArrayPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            registry.TryGetTransferManager(out var transferManager)
                ? transferManager.UploadAsync(byteArrayPart, optionsBuilder)
                : registry.GetUploadManager().UploadAsync(byteArrayPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its bytes data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="byteArrayPart">The file bytes data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> UploadAsync<TUploadApi>(
            this IApizrEnumerableRegistry registry, ByteArrayPart byteArrayPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) where TUploadApi : IUploadApi<HttpResponseMessage>
            => registry.GetUploadManagerFor<TUploadApi>().UploadAsync(byteArrayPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its bytes data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="byteArrayPart">The file bytes data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<TUploadApiResultData> UploadAsync<TUploadApi, TUploadApiResultData>(
            this IApizrEnumerableRegistry registry, ByteArrayPart byteArrayPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) where TUploadApi : IUploadApi<TUploadApiResultData>
            => registry.GetUploadManagerFor<TUploadApi, TUploadApiResultData>().UploadAsync(byteArrayPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its stream data
        /// </summary>
        /// <param name="registry">The registry</param>
        /// <param name="streamPart">The file stream data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> UploadAsync(this IApizrEnumerableRegistry registry, StreamPart streamPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            registry.TryGetTransferManager(out var transferManager)
                ? transferManager.UploadAsync(streamPart, optionsBuilder)
                : registry.GetUploadManager().UploadAsync(streamPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its stream data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="streamPart">The file stream data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> UploadAsync<TUploadApi>(this IApizrEnumerableRegistry registry, StreamPart streamPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) where TUploadApi : IUploadApi<HttpResponseMessage>
            => registry.GetUploadManagerFor<TUploadApi>().UploadAsync(streamPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its stream data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="streamPart">The file stream data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<TUploadApiResultData> UploadAsync<TUploadApi, TUploadApiResultData>(this IApizrEnumerableRegistry registry, StreamPart streamPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) where TUploadApi : IUploadApi<TUploadApiResultData>
            => registry.GetUploadManagerFor<TUploadApi, TUploadApiResultData>().UploadAsync(streamPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its file info data
        /// </summary>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfoPart">The file info data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> UploadAsync(this IApizrEnumerableRegistry registry, FileInfoPart fileInfoPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            registry.TryGetTransferManager(out var transferManager)
                ? transferManager.UploadAsync(fileInfoPart, optionsBuilder)
                : registry.GetUploadManager().UploadAsync(fileInfoPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its file info data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfoPart">The file info data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> UploadAsync<TUploadApi>(this IApizrEnumerableRegistry registry, FileInfoPart fileInfoPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) where TUploadApi : IUploadApi<HttpResponseMessage>
            => registry.GetUploadManagerFor<TUploadApi>().UploadAsync(fileInfoPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its file info data
        /// </summary>
        /// <typeparam name="TUploadApi">The upload api type to manage</typeparam>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfoPart">The file info data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<TUploadApiResultData> UploadAsync<TUploadApi, TUploadApiResultData>(this IApizrEnumerableRegistry registry, FileInfoPart fileInfoPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) where TUploadApi : IUploadApi<TUploadApiResultData>
            => registry.GetUploadManagerFor<TUploadApi, TUploadApiResultData>().UploadAsync(fileInfoPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its bytes data
        /// </summary>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="byteArrayPart">The file bytes data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<TUploadApiResultData> UploadWithAsync<TUploadApiResultData>(
            this IApizrEnumerableRegistry registry, ByteArrayPart byteArrayPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetUploadManagerFor<IUploadApi<TUploadApiResultData>, TUploadApiResultData>().UploadAsync(byteArrayPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its stream data
        /// </summary>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="streamPart">The file stream data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<TUploadApiResultData> UploadWithAsync<TUploadApiResultData>(this IApizrEnumerableRegistry registry, StreamPart streamPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetUploadManagerFor<IUploadApi<TUploadApiResultData>, TUploadApiResultData>().UploadAsync(streamPart, optionsBuilder);

        /// <summary>
        /// Upload a file from its file info data
        /// </summary>
        /// <typeparam name="TUploadApiResultData">The upload api return type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfoPart">The file info data</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<TUploadApiResultData> UploadWithAsync<TUploadApiResultData>(this IApizrEnumerableRegistry registry, FileInfoPart fileInfoPart,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetUploadManagerFor<IUploadApi<TUploadApiResultData>, TUploadApiResultData>().UploadAsync(fileInfoPart, optionsBuilder);

        #endregion

        #region Download

        /// <summary>
        /// Download a file
        /// </summary>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> DownloadAsync(this IApizrEnumerableRegistry registry, FileInfo fileInfo,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            registry.TryGetTransferManager(out var transferManager)
                ? transferManager.DownloadAsync(fileInfo, optionsBuilder)
                : registry.GetDownloadManager().DownloadAsync(fileInfo, optionsBuilder);

        /// <summary>
        /// Download a file with custom query parameters
        /// </summary>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some custom query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> DownloadAsync(this IApizrEnumerableRegistry registry, FileInfo fileInfo,
            IDictionary<string, object> downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) =>
            registry.TryGetTransferManager(out var transferManager)
                ? transferManager.DownloadAsync(fileInfo, optionsBuilder)
                : registry.GetDownloadManager().DownloadAsync(fileInfo, downloadParams, optionsBuilder);

        /// <summary>
        /// Download a file
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> DownloadAsync<TDownloadApi>(this IApizrEnumerableRegistry registry, FileInfo fileInfo,
            Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi<IDictionary<string, object>>
            => registry.GetDownloadManagerFor<TDownloadApi>().DownloadAsync(fileInfo, optionsBuilder);

        /// <summary>
        /// Download a file with custom query parameters
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some custom query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> DownloadAsync<TDownloadApi>(this IApizrEnumerableRegistry registry, FileInfo fileInfo,
            IDictionary<string, object> downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi<IDictionary<string, object>>
            => registry.GetDownloadManagerFor<TDownloadApi>().DownloadAsync(fileInfo, downloadParams, optionsBuilder);

        /// <summary>
        /// Download a file
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
        /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> DownloadAsync<TDownloadApi, TDownloadParams>(this IApizrEnumerableRegistry registry,
            FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi<TDownloadParams>
            => registry.GetDownloadManagerFor<TDownloadApi, TDownloadParams>().DownloadAsync(fileInfo, optionsBuilder);

        /// <summary>
        /// Download a file with custom query parameters
        /// </summary>
        /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
        /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some custom query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> DownloadAsync<TDownloadApi, TDownloadParams>(this IApizrEnumerableRegistry registry,
            FileInfo fileInfo, TDownloadParams downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            where TDownloadApi : IDownloadApi<TDownloadParams>
            => registry.GetDownloadManagerFor<TDownloadApi, TDownloadParams>()
                .DownloadAsync(fileInfo, downloadParams, optionsBuilder);

        /// <summary>
        /// Download a file
        /// </summary>
        /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> DownloadWithAsync<TDownloadParams>(this IApizrEnumerableRegistry registry,
            FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetDownloadManagerFor<IDownloadApi<TDownloadParams>, TDownloadParams>().DownloadAsync(fileInfo, optionsBuilder);

        /// <summary>
        /// Download a file with custom query parameters
        /// </summary>
        /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
        /// <param name="registry">The registry</param>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some custom query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        /// <returns></returns>
        public static Task<FileInfo> DownloadWithAsync<TDownloadParams>(this IApizrEnumerableRegistry registry,
            FileInfo fileInfo, TDownloadParams downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null)
            => registry.GetDownloadManagerFor<IDownloadApi<TDownloadParams>, TDownloadParams>()
                .DownloadAsync(fileInfo, downloadParams, optionsBuilder);


        #endregion
    }
}
