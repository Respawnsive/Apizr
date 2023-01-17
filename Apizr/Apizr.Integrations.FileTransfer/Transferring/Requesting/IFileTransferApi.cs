using System.Collections.Generic;

namespace Apizr.Transferring.Requesting
{
    public interface IFileTransferApi<in TDownloadParams> : IDownloadApi<TDownloadParams>, IUploadApi { }

    public interface IFileTransferApi : IFileTransferApi<IDictionary<string, object>> { }
}
