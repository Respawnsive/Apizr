using System.Text.Json.Serialization;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;

namespace Apizr.Tests.Models
{
    [AutoRegister("https://reqres.in/api/users")]
    public record UserDetails
    {
        [JsonPropertyName("data")]
        public User User { get; init; }

        [JsonPropertyName("ad")]
        public UserAd Ad { get; init; }
    }
}
