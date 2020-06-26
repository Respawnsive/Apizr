using System.Collections.Generic;
using Apizr.Requesting;
using Newtonsoft.Json;

namespace Apizr.Sample.Api.Models
{
    public class PagedResult<T> : IPagedResult<T> where T : class
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("per_page")]
        public int PerPage { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("data")]
        public IEnumerable<T> Data { get; set; }
    }
}
