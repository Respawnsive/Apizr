using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Resiliencing.Attributes;
using Apizr.Resiliencing.Attributes.Http;
using Apizr.Tests.Models;
using Fusillade;
using Microsoft.Extensions.Logging;
using Refit;

[assembly:GetResiliencePipeline("TransientHttpError")]
[assembly:Cache(CacheMode.FetchOrGet, "00:10:00")]
//[assembly:Timeout("00:00:02")]
//[assembly:Log(HttpMessageParts.All, HttpTracerMode.Everything, LogLevel.Trace)]
[assembly:Priority(Priority.Background)]
namespace Apizr.Tests.Apis
{
    [//BaseAddress("https://reqres.in/api"), 
     AutoRegister("https://reqres.in/api"),
     Log(HttpMessageParts.RequestAll, HttpTracerMode.ErrorsAndExceptionsOnly, LogLevel.Information),
     Headers("testKey1: *testValue1*", "testKey2: testValue2.1"),
     Cache(CacheMode.FetchOrGet, "00:09:00"),
     PostResiliencePipeline("TransientHttpError2"),
     Priority(Priority.Speculative)]//, Timeout("00:00:04")]
    public interface IReqResUserService
    {
        [Get("/users")]
        Task<IApiResponse<ApiResult<User>>> SafeGetUsersAsync();

        [Get("/users")]
        Task<ApiResponse<ApiResult<User>>> SafeGetUsersAsync([Property(nameof(HttpStatusCode))] HttpStatusCode statusCode);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync();

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync([Property(nameof(HttpStatusCode))] HttpStatusCode statusCode);

        [Get("/users"), Cache(CacheMode.FetchOrGet, "00:08:00")]
        Task<ApiResult<User>> GetUsersAsync([Property(nameof(HttpStatusCode))] HttpStatusCode statusCode, [RequestOptions] IApizrRequestOptions options);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync([Property(nameof(Task.Delay))] TimeSpan delay, [RequestOptions] IApizrRequestOptions options);

        [Get("/users"), 
         Log(HttpMessageParts.RequestBody, HttpTracerMode.ExceptionsOnly, LogLevel.Warning),
         Priority(Priority.UserInitiated), 
         Cache(CacheMode.FetchOrGet, "00:08:00")]
        Task<ApiResult<User>> GetUsersAsync([RequestOptions] IApizrRequestOptions options);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync([Priority] int priority);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(CancellationToken cancellationToken);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(bool isTest);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(IDictionary<string, object> parameters);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(ReadAllUsersParams parameters);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(bool isTest, IDictionary<string, object> parameters);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync([CacheKey] bool isTest, IDictionary<string, object> parameters, CancellationToken cancellationToken);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(IDictionary<string, object> userIds, CancellationToken cancellationToken);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(ReadAllUsersParams parameters, CancellationToken cancellationToken);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(IDictionary<string, object> userIds, ReadAllUsersParams parameters, CancellationToken cancellationToken);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(bool isTest, IDictionary<string, object> userIds, ReadAllUsersParams parameters, CancellationToken cancellationToken);

        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync(bool isTest, IDictionary<string, object> userIds, ReadAllUsersParams parameters, [Priority] int priority, CancellationToken cancellationToken);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, [Priority] int priority, CancellationToken cancellationToken);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, IDictionary<string, object> parameters, [Priority] int priority, CancellationToken cancellationToken);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync([CacheKey] int userId, IDictionary<string, object> parameters, [CacheKey(nameof(ReadAllUsersParams.Param1))] ReadAllUsersParams customParams, [Property(nameof(HttpStatusCode))] HttpStatusCode statusCode);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId);

        [Get("/users/{userId}")]
        Task<UserDetails> GetUserAsync(int userId, [Property(nameof(HttpStatusCode))] HttpStatusCode statusCode);

        [Get("/users/{userId}")]
        Task<ApiResponse<UserDetails>> GetUserResponseAsync(int userId);

        [Get("/users/{userId}")]
        Task<ApiResponse<UserDetails>> GetUserResponseAsync(int userId, [Property(nameof(HttpStatusCode))] HttpStatusCode statusCode);

        [Post("/users")]
        Task<User> CreateUser(User user, CancellationToken cancellationToken);

        [Get("/users")]
        Task<ApiResult<User>> GetDelayedUsersAsync([Query] int delay, [RequestOptions] IApizrRequestOptions options);

        [Get("/users")]//, Timeout("00:00:06")]
        Task<ApiResult<User>> GetDelayedUsersAsync([Query] int delay, CancellationToken cancellationToken);

        [Get("/users")]//, Timeout("00:00:06")]
        Task<HttpResponseMessage> GetDelayedUsersAsync([Query] int delay);
    }
}
