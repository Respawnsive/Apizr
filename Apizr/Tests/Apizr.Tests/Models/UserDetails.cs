using System.Text.Json.Serialization;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;

namespace Apizr.Tests.Models
{
    [AutoRegister("https://reqres.in/api/users")]
    public class UserDetails
    {
        [JsonPropertyName("data")]
        public User User { get; set; }

        [JsonPropertyName("ad")]
        public UserAd Ad { get; set; }
    }
}
