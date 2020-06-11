using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Apizr.Sample.Api.Models
{
    public class UserAd
    {
        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
