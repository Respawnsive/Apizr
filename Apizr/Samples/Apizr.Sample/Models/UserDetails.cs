using System.Text.Json.Serialization;
using Apizr.Requesting;

namespace Apizr.Sample.Models
{
    [CrudEntity("https://reqres.in/api/users")]
    public class UserDetails
    {
        [JsonPropertyName("data")]
        public User User { get; set; }

        [JsonPropertyName("ad")]
        public UserAd Ad { get; set; }
    }
}
