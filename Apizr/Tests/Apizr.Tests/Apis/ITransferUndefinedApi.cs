using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Tests.Apis
{
    [Headers("testKey1: testValue1")]
    public interface ITransferUndefinedApi : ITransferApi
    { }
}
