using System.Collections.Generic;

namespace Apizr.Transferring.Requesting
{
    public interface ITransferApi<in TDownloadParams> : IDownloadApi<TDownloadParams>, IUploadApi { }
    
    public interface ITransferApi : ITransferApi<IDictionary<string, object>>, IDownloadApi { }
}
