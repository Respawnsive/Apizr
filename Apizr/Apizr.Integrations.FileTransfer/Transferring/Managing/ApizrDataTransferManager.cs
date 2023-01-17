using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public class ApizrDataTransferManager<TDataTransferApi> : IApizrDataTransferManager<TDataTransferApi> where TDataTransferApi : IDataTransferApi
{
    protected readonly IApizrManager<TDataTransferApi> TransferApiManager;

    public ApizrDataTransferManager(IApizrManager<TDataTransferApi> transferApiManager)
    {
        TransferApiManager = transferApiManager;
    }
}