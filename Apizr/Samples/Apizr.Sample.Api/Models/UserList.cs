using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Apizr.Sample.Api.Models
{
    public class UserList
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("data")]
        public List<User> Data { get; set; }
    }
}
