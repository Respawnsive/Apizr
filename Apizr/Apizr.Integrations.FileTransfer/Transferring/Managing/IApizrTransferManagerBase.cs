using Apizr.Transferring.Requesting;

namespace Apizr.Transferring.Managing;

public interface IApizrTransferManagerBase<TTransferApiBase> : IApizrManager where TTransferApiBase : ITransferApiBase
{
}