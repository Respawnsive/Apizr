using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public interface IApizrDataTransferManager<TDataTransferApi> : IApizrManager where TDataTransferApi : IDataTransferApi
{
}