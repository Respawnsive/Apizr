using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Integrations.Fusillade;
using Apizr.Logging;
using Apizr.Policing;
using Apizr.Sample.Api.Models;
using HttpTracer;
using Refit;

[assembly:Policy("TransientHttpError")]
[assembly:CacheIt(CacheMode.GetAndFetch, "00:10:00")]
[assembly:LogIt(HttpMessageParts.All, ApizrLogLevel.High)]
namespace Apizr.Sample.Api
{
    [WebApi("https://reqres.in/api")]
    public interface IReqResService
    {
        [Get("/users")]
        Task<UserList> GetUsersAsync([Priority] int priority);

        [Get("/users")]
        Task<UserList> GetUsersAsync(CancellationToken cancellationToken);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId, CancellationToken cancellationToken);

        [Post("/users")]
        Task<User> CreateUser(User user, CancellationToken cancellationToken);

        [Get("/users")]
        Task<UserList> GetUsersAsync([CacheKey] IDictionary<string, string> userIds, CancellationToken cancellationToken);

        [Get("/users")]
        Task<UserList> GetUsersAsync([CacheKey] ReadAllUsersParams parameters, CancellationToken cancellationToken);
    }
}
