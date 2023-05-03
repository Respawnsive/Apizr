using Newtonsoft.Json;
using System;

namespace Apizr.Sample.Console.Models.Uploads
{
    public class UploadResult
    {
        [JsonProperty("args")]
        public Args Args { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("files")]
        public Files Files { get; set; }

        [JsonProperty("form")]
        public Args Form { get; set; }

        [JsonProperty("headers")]
        public Headers Headers { get; set; }

        [JsonProperty("json")]
        public object Json { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}
