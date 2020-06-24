using Apizr.Requesting;
using Newtonsoft.Json;

namespace Apizr.Sample.Api.Models
{
    [Crud("https://reqres.in/api/users", typeof(int))]
    public class UserDetails
    {
        [JsonProperty("data")]
        public User User { get; set; }

        [JsonProperty("ad")]
        public UserAd Ad { get; set; }
    }
}
