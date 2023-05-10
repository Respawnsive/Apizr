using System.Collections.Generic;
using System.Net.Http;

namespace Apizr.Transferring.Requesting
{
    public interface ITransferApi<in TDownloadParams, TUploadApiResultData> : IDownloadApi<TDownloadParams>, IUploadApi<TUploadApiResultData> { }

    // This one can't be used yet because of a Refit issue
    //public interface ITransferApi<in TDownloadParams> : ITransferApi<TDownloadParams, HttpResponseMessage>, IUploadApi { }

    public interface ITransferApi : ITransferApi<IDictionary<string, object>, HttpResponseMessage>, IDownloadApi, IUploadApi { }
}
