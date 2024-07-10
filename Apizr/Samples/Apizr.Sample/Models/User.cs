using System.Collections.Generic;
using System.Text.Json.Serialization;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Requesting;

namespace Apizr.Sample.Models
{
    [AutoRegister<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>("https://reqres.in/api/users")]
    [CacheReadAll(CacheMode.GetAndFetch)]
    [CacheRead(CacheMode.GetOrFetch)]
    [Log(HttpMessageParts.All)]
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
