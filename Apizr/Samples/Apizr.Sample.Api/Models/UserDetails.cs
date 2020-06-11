using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Apizr.Sample.Api.Models
{
    public class UserDetails
    {
        [JsonProperty("data")]
        public User User { get; set; }

        [JsonProperty("ad")]
        public UserAd Ad { get; set; }
    }
}
