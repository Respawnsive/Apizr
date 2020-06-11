using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Policing;
using Apizr.Sample.Api.Models;
using Apizr.Tracing;
using Refit;

[assembly:Policy("TransientHttpError")]
namespace Apizr.Sample.Api
{
    [WebApi("https://reqres.in/"), Cache, Trace]
    public interface IReqResService
    {
        [Get("/api/users")]
        Task<UserList> GetUsersAsync(CancellationToken cancellationToken);

        [Get("/api/users/{userId}")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId, CancellationToken cancellationToken);
    }
}
