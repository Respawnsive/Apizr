using Apizr.Requesting;
using Newtonsoft.Json;

namespace Apizr.Sample.Api.Models
{
    [CrudEntity("https://reqres.in/api/users")]
    public class UserDetails
    {
        [JsonProperty("data")]
        public User User { get; set; }

        [JsonProperty("ad")]
        public UserAd Ad { get; set; }
    }
}
