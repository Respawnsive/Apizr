using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Apizr.Tests.Models;
using Refit;

namespace Apizr.Tests.Apis
{
    [BaseAddress("https://reqres.in/api"), Headers("testKey1: testValue1")]
    public interface IReqResSecondSimpleService
    {
        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync();

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync([RequestOptions] IApizrRequestOptions options);
    }
}
