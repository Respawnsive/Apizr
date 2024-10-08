﻿using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Apizr.Resiliencing.Attributes;
using Apizr.Resiliencing.Attributes.Http;
using Apizr.Tests.Models;
using Refit;

namespace Apizr.Tests.Apis
{
    [BaseAddress("https://reqres.in/api"), Headers("testKey1: *testValue1*", "testKey2: testValue2.1"), PostResiliencePipeline("TransientHttpError2")]
    public interface IReqResSimpleService
    {
        [Get("/users")]
        Task<ApiResult<User>> GetUsersAsync();

        [Get("/users"), Headers("testStoreKey1: *{0}*", "testStoreKey2: {0}", "testKeyOver1: testValueOver1.1"), ResiliencePipeline("TestPipeline1")]
        Task<ApiResult<User>> GetUsersAsync([RequestOptions] IApizrRequestOptions options);
    }
}
