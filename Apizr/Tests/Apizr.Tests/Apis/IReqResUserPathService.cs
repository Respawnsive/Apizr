using Apizr.Tests.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Apizr.Tests.Apis
{
    [WebApi("users")]
    public interface IReqResUserPathService
    {
        [Get("")]
        Task<ApiResult<User>> GetUsersAsync();

        [Get("/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, [Priority] int priority, CancellationToken cancellationToken);
    }
}
