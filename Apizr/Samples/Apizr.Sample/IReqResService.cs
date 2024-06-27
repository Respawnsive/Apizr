using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Apizr.Logging.Attributes;
using Apizr.Resiliencing.Attributes;
using Apizr.Sample.Models;
using Refit;

[assembly:ResiliencePipeline("TransientHttpError")]
[assembly:Cache(CacheMode.GetAndFetch, "00:10:00")]
[assembly:Log]
namespace Apizr.Sample
{
    [BaseAddress("https://reqres.in/api")]//, Log(HttpMessageParts.None, LogLevel.None)]
    public interface IReqResService
    {
        [Get("/users")]
        Task<UserList> GetUsersAsync();

        [Get("/users")]
        Task<UserList> GetUsersAsync([RequestOptions] IApizrRequestOptions options);

        [Get("/users")]
        Task<UserList> GetUsersAsync([Property(nameof(HttpStatusCode))] HttpStatusCode statusCode);

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

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, [RequestOptions] IApizrRequestOptions options);

        [Post("/users")]
        Task<User> CreateUser(User user, CancellationToken cancellationToken);
    }
}
