using System.Collections.Generic;
using System.Net.Http;

namespace Apizr.Transferring.Requesting
{
    public interface ITransferApi<in TDownloadParams, TUploadApiResultData> : IDownloadApi<TDownloadParams>, IUploadApi<TUploadApiResultData> { }

    public interface ITransferApi<in TDownloadParams> : ITransferApi<TDownloadParams, HttpResponseMessage>, IUploadApi { }

    public interface ITransferApi : ITransferApi<IDictionary<string, object>>, IDownloadApi { }
}
