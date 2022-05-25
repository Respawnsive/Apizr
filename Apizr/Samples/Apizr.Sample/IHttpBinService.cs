using System.Net.Http;
using System.Threading.Tasks;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Refit;

namespace Apizr.Sample
{
    [WebApi("https://httpbin.org", false), Log(HttpMessageParts.RequestHeaders)]
    public interface IHttpBinService
    {
        [Get("/bearer")]
        [Headers("Authorization: Bearer")]
        [Cache(CacheMode.None)]
        Task<HttpResponseMessage> AuthBearerAsync();

        [Multipart]
        [Post("/post")]
        [Cache(CacheMode.None)]
        Task<HttpResponseMessage> UploadStreamPart(StreamPart stream);
    }
}
