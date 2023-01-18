using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public abstract class ApizrTransferManagerBase<TTransferApiBase> : IApizrTransferManagerBase<TTransferApiBase> where TTransferApiBase : ITransferApiBase
{
    protected readonly IApizrManager<TTransferApiBase> TransferApiManager;

    protected ApizrTransferManagerBase(IApizrManager<TTransferApiBase> transferApiManager)
    {
        TransferApiManager = transferApiManager;
    }
}