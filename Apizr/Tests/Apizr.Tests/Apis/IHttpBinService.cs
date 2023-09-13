using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring.Request;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Refit;

namespace Apizr.Tests.Apis
{
    [WebApi("https://httpbin.org", false), Log(HttpMessageParts.None)]
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
    }
}
