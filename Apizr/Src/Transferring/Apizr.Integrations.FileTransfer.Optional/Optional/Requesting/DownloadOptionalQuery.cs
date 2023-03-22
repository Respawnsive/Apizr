using System;
using System.Collections.Generic;
using System.IO;
using Apizr.Configuring.Request;
using Apizr.Mediation.Querying;
using Apizr.Transferring.Requesting;
using Optional;

namespace Apizr.Optional.Requesting
{
    /// <summary>
    /// The mediation download query with a custom query parameters type
    /// </summary>
    /// <typeparam name="TDownloadApi">The download api type to manage</typeparam>
    /// <typeparam name="TDownloadParams">The query parameters type</typeparam>
    public class DownloadOptionalQuery<TDownloadApi, TDownloadParams> : MediationQueryBase<Option<FileInfo, ApizrException>, IApizrRequestOptions, IApizrRequestOptionsBuilder>
        where TDownloadApi : IDownloadApi<TDownloadParams>
    {
        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some custom query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        public DownloadOptionalQuery(FileInfo fileInfo, TDownloadParams downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(optionsBuilder)
        {
            FileInfo = fileInfo;
            DownloadParams = downloadParams;
        }

        /// <summary>
        /// Some information about the file to download
        /// </summary>
        public FileInfo FileInfo { get; }

        /// <summary>
        /// Some custom query parameters
        /// </summary>
        public TDownloadParams DownloadParams { get; }
    }

    /// <summary>
    /// The mediation download query with a dictionary query parameters type
    /// </summary>
    public class DownloadOptionalQuery<TDownloadApi> : DownloadOptionalQuery<TDownloadApi, IDictionary<string, object>>
        where TDownloadApi : IDownloadApi
    {
        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        public DownloadOptionalQuery(FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(fileInfo, null, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        public DownloadOptionalQuery(FileInfo fileInfo, IDictionary<string, object> downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(fileInfo, downloadParams, optionsBuilder)
        {
        }
    }

    /// <summary>
    /// The mediation download query with a dictionary query parameters type
    /// </summary>
    public class DownloadOptionalQuery : DownloadOptionalQuery<IDownloadApi>
    {
        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="optionsBuilder">Some request options</param>
        public DownloadOptionalQuery(FileInfo fileInfo, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(fileInfo, null, optionsBuilder)
        {
        }

        /// <summary>
        /// The mediation Read query constructor
        /// </summary>
        /// <param name="fileInfo">Some information about the file to download</param>
        /// <param name="downloadParams">Some query parameters</param>
        /// <param name="optionsBuilder">Some request options</param>
        public DownloadOptionalQuery(FileInfo fileInfo, IDictionary<string, object> downloadParams, Action<IApizrRequestOptionsBuilder> optionsBuilder = null) : base(fileInfo, downloadParams, optionsBuilder)
        {
        }
    }
}
