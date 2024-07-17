using System;
using System.Text.Json.Serialization;

namespace Apizr.Tests.Models
{
    public record UserAd
    {
        [JsonPropertyName("company")]
        public string Company { get; init; }

        [JsonPropertyName("url")]
        public Uri Url { get; init; }

        [JsonPropertyName("text")]
        public string Text { get; init; }
    }
}
