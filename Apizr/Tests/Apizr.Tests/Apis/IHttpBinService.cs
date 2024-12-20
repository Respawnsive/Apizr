﻿using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring;
using Apizr.Configuring.Request;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Resiliencing.Attributes;
using Microsoft.Extensions.Logging;
using Refit;

namespace Apizr.Tests.Apis
{
    [AutoRegister("https://httpbin.org"), 
     Log(HttpMessageParts.None, HttpTracerMode.ExceptionsOnly, LogLevel.Critical), 
     Headers("testKey1: testValue1"), 
     ResiliencePipeline("TransientHttpError")]
    public interface IHttpBinService
    {
        [Get("/bearer")]
        [Headers("Authorization: Bearer")]
        [Cache(CacheMode.None)]
        Task<HttpResponseMessage> AuthBearerAsync();

        [Multipart]
        [Post("/post")]
        Task UploadAsync([AliasAs("file")] StreamPart streamPart);

        [Multipart]
        [Post("/post")]
        Task UploadAsync([AliasAs("file")] StreamPart streamPart, [RequestOptions] IApizrRequestOptions options);

        [Get("/status/{statusCode}")]
        Task<string> GetStatusAsync(int statusCode);

        [Get("/status/{statusCode}")]
        Task<string> GetStatusAsync(int statusCode, [RequestOptions] IApizrRequestOptions options);
    }
}
