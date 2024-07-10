using System.Text.Json.Serialization;
using Apizr.Configuring;
using Apizr.Requesting;
using Apizr.Requesting.Attributes;

namespace Apizr.Sample.Models
{
    [BaseAddress("https://reqres.in/api/users")]
    public class UserDetails
    {
        [JsonPropertyName("data")]
        public User User { get; set; }

        [JsonPropertyName("ad")]
        public UserAd Ad { get; set; }
    }
}
