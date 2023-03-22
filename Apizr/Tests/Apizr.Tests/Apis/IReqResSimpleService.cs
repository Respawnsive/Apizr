using System.Threading.Tasks;
using Apizr.Tests.Models;
using Refit;

namespace Apizr.Tests.Apis
{
    [WebApi("https://reqres.in/api")]
    public interface IReqResSimpleService
    {
        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync();
    }
}
