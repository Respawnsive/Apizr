using System.Collections.Generic;
using System.Text.Json.Serialization;
using Apizr.Caching;
using Apizr.Caching.Attributes;
using Apizr.Configuring;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;
using Fusillade;

namespace Apizr.Tests.Models
{
    [AutoRegister<ICrudApi<User, int, PagedResult<User>, IDictionary<string, object>>>("https://reqres.in/api/users"),
    CacheReadAll(CacheMode.FetchOrGet),
    CacheRead(CacheMode.GetOrFetch),
    ReadAllHeaders("testKey1: testValue1", "testKey2: *testValue2*", "testKey3: {0}", "testKey4: *{0}*"),
    //Log(HttpMessageParts.All),
    ReadAllPriority(Priority.Background)]
    public record User
    {
        [JsonPropertyName("id")] public int Id { get; init; }

        [JsonPropertyName("first_name")] public string FirstName { get; init; }

        [JsonPropertyName("last_name")] public string LastName { get; init; }

        [JsonPropertyName("avatar")] public string Avatar { get; init; }

        [JsonPropertyName("email")] public string Email { get; init; }
    }
}
