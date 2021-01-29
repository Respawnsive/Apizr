using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
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
    [WebApi("https://reqres.in/", false)]
    public interface IReqResService
    {
        [Get("/api/users")]
        Task<UserList> GetUsersAsync([Property("SomeKey")] string someValue);

        [Get("/api/users")]
        Task<UserList> GetUsersAsync(CancellationToken cancellationToken);

        [Get("/api/users/{userId}")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId, CancellationToken cancellationToken);

        [Post("/api/users")]
        Task<User> CreateUser(User user, CancellationToken cancellationToken);

        [Get("/api/users")]
        Task<UserList> GetUsersAsync([CacheKey] IDictionary<string, string> userIds, CancellationToken cancellationToken);

        [Get("/api/users")]
        Task<UserList> GetUsersAsync([CacheKey] ReadAllUsersParams parameters, CancellationToken cancellationToken);
    }
}
