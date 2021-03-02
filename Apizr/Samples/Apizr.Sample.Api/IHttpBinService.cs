using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Logging;
using HttpTracer;
using Refit;

namespace Apizr.Sample.Api
{
    [WebApi("https://httpbin.org/", false), LogIt(HttpMessageParts.None, ApizrLogLevel.Low)]
    public interface IHttpBinService
    {
        [Get("/bearer")]
        [Headers("Authorization: Bearer")]
        [CacheIt(CacheMode.None)]
        Task<HttpResponseMessage> AuthBearerAsync();
    }
}
