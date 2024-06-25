using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Cancelling.Attributes.Operation;
using Apizr.Cancelling.Attributes.Request;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Resiliencing.Attributes;
using Apizr.Resiliencing.Attributes.Http;
using Apizr.Tests.Models;
using Microsoft.Extensions.Logging;
using Refit;

//[assembly: GetResiliencePipeline("TransientHttpError")]
//[assembly: Cache(CacheMode.GetAndFetch, "00:05:00")]
//[assembly: OperationTimeout("00:05:00")]
//[assembly: Log(HttpMessageParts.All, HttpTracerMode.Everything, LogLevel.Trace)]
namespace Apizr.Tests.Apis
{
    [BaseAddress("https://reqres.in/api"), 
     Headers("testKey1: testValue1"), 
     PostResiliencePipeline("TransientHttpError2"),
     Cache(CacheMode.GetAndFetch, "00:04:00"),
     OperationTimeout("00:04:00"),
     Log(HttpMessageParts.AllButBodies, HttpTracerMode.Everything, LogLevel.Trace)]
    public interface IReqResParentApi
    {
        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync();

        [Get("/users"), 
         Headers("testKey1: testValue2"), 
         ResiliencePipeline("TestPipeline1"), 
         Cache(CacheMode.GetAndFetch, "00:03:00"),
         RequestTimeout("00:01:00"),
         Log(HttpMessageParts.HeadersOnly, HttpTracerMode.Everything, LogLevel.Trace)]
        Task<ApiResult<User>> GetUsersAsync([RequestOptions] IApizrRequestOptions options);
    }


    [Headers("testKey1: testValue3"),
     PutResiliencePipeline("TransientHttpError3"),
     Cache(CacheMode.GetAndFetch, "00:02:00"),
     OperationTimeout("00:03:00"),
     Log(HttpMessageParts.AllButBodies, HttpTracerMode.Everything, LogLevel.Trace)]
    public interface IReqResChildApi : IReqResParentApi
    {
    }
}
