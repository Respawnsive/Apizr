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
//[assembly:CacheIt(CacheMode.GetAndFetch, "00:10:00")]
[assembly:LogIt(HttpMessageParts.All, ApizrLogLevel.High)]
namespace Apizr.Sample.Api
{
    [WebApi("https://reqres.in/api")]
    public interface IReqResService
    {
        [Get("/users")]
        Task<UserList> GetUsersAsync();

        [Get("/users")]
        Task<UserList> GetUsersAsync([Priority] int priority);

        [Get("/users")]
        Task<UserList> GetUsersAsync(CancellationToken cancellationToken);

        [Get("/users")]
        Task<UserList> GetUsersAsync(bool isTest);

        [Get("/users")]
        Task<UserList> GetUsersAsync(IDictionary<string, object> parameters);

        [Get("/users")]
        Task<UserList> GetUsersAsync(ReadAllUsersParams parameters);

        [Get("/users")]
        Task<UserList> GetUsersAsync(bool isTest, IDictionary<string, object> parameters);

        [Get("/users")]
        Task<UserList> GetUsersAsync([CacheKey] bool isTest, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        [Get("/users")]
        Task<UserList> GetUsersAsync(IDictionary<string, object> userIds, CancellationToken cancellationToken);

        [Get("/users")]
        Task<UserList> GetUsersAsync(ReadAllUsersParams parameters, CancellationToken cancellationToken);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, IDictionary<string, object> parameters, [Priority] int priority, CancellationToken cancellationToken);

        [Post("/users")]
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
    }
}
