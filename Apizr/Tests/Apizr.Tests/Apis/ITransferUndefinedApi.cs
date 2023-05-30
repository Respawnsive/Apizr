using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Tests.Apis
{
    [WebApi, Headers("testKey1: testValue1")]
    public interface ITransferUndefinedApi : ITransferApi
    { }
}
