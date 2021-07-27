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
    [WebApi("https://httpbin.org/", false), Log(HttpMessageParts.None)]
    public interface IHttpBinService
    {
        [Get("/bearer")]
        [Headers("Authorization: Bearer")]
        [Cache(CacheMode.None)]
        Task<HttpResponseMessage> AuthBearerAsync();
    }
}
