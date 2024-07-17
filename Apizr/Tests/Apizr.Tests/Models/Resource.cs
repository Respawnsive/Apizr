using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apizr.Tests.Models
{
    public record Resource
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("year")]
        public string Year { get; init; }

        [JsonPropertyName("color")]
        public string Color { get; init; }

        [JsonPropertyName("pantone_value")]
        public string PantoneValue { get; init; }
    }
}
