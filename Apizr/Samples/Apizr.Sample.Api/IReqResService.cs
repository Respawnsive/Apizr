using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Policing;
using Apizr.Sample.Api.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Refit;

[assembly:Policy("TransientHttpError")]
[assembly:Cache(CacheMode.GetAndFetch, "00:10:00")]
//[assembly:Log]
namespace Apizr.Sample.Api
{
    [WebApi("https://reqres.in/api")]//, Log(HttpMessageParts.None, LogLevel.None)]
    public interface IReqResService
    {
        [Get("/users")]
        Task<UserList> GetUsersAsync();

        [Get("/users")]
        Task<UserList> GetUsersAsync([Priority] int priority);

        [Get("/users"), Log(HttpMessageParts.RequestBody, HttpTracerMode.ErrorsAndExceptionsOnly, LogLevel.Critical)]
        Task<UserList> GetUsersAsync([Priority] int priority, [Context] Context context);

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

        [Get("/users")]
        Task<UserList> GetUsersAsync(IDictionary<string, object> userIds, ReadAllUsersParams parameters, CancellationToken cancellationToken);

        [Get("/users")]
        Task<UserList> GetUsersAsync(bool isTest, IDictionary<string, object> userIds, ReadAllUsersParams parameters, CancellationToken cancellationToken);

        [Get("/users")]
        Task<UserList> GetUsersAsync(bool isTest, IDictionary<string, object> userIds, ReadAllUsersParams parameters, [Priority] int priority, CancellationToken cancellationToken);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, [Priority] int priority, CancellationToken cancellationToken);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, IDictionary<string, object> parameters, [Priority] int priority, CancellationToken cancellationToken);

        [Post("/users")]
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
    }
}
