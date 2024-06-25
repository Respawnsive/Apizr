using Apizr.Tests.Models;
using Refit;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Configuring;

namespace Apizr.Tests.Apis
{
    [BaseAddress("users")]
    public interface IReqResUserPathService
    {
        [Get("")]
        Task<ApiResult<User>> GetUsersAsync();

        [Get("/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, [Priority] int priority, CancellationToken cancellationToken);
    }
}
