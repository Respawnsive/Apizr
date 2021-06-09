using System.Text.Json.Serialization;
using Apizr.Caching;
using Apizr.Requesting;
using Apizr.Tracing;
using HttpTracer;

namespace Apizr.Sample.Api.Models
{
    [CrudEntity("https://reqres.in/api/users", typeof(int), typeof(PagedResult<>))]
    [CacheReadAll(CacheMode.GetAndFetch)]
    [CacheRead(CacheMode.GetOrFetch)]
    [Trace(HttpMessageParts.None)]
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
