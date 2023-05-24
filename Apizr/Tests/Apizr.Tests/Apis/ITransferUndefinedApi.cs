using Apizr.Transferring.Requesting;
using Refit;

namespace Apizr.Tests.Apis
{
    [WebApi, Headers("testKey: testValue")]
    public interface ITransferUndefinedApi : ITransferApi
    { }
}
