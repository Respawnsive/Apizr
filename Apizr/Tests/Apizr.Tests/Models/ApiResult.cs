using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Apizr.Tests.Models
{
    public record ApiResult<TData> where TData : class, new()
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
        public List<TData> Data { get; init; }
    }
}
