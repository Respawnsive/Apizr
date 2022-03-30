using System;
using System.Text.Json.Serialization;

namespace Apizr.Tests.Models
{
    public class UserAd
    {
        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
