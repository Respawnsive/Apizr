using System.Threading.Tasks;
using Apizr.Configuring.Request;
using Apizr.Tests.Models;
using Refit;

namespace Apizr.Tests.Apis
{
    [WebApi("https://reqres.in/api"), Headers("testKey1: *testValue1*", "testKey2: testValue2.1")]
    public interface IReqResSimpleService
    {
        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync();

        [Get("/users"), Headers("testStoreKey1: *{0}*", "testStoreKey2: {0}")]
        Task<ApiResult<User>> GetUsersAsync([RequestOptions] IApizrRequestOptions options);
    }
}
