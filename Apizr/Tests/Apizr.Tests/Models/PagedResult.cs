using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Apizr.Tests.Models
{
    public record PagedResult<T> where T : class
    {
        [JsonPropertyName("page")]
        public int Page { get; init; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; init; }

        [JsonPropertyName("total")]
        public int Total { get; init; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; init; }

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; init; }
    }
}
