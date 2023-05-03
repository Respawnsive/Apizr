using Newtonsoft.Json;

namespace Apizr.Sample.Console.Models.Uploads;

public class Headers
{
    [JsonProperty("Content-Length")]
    public string ContentLength { get; set; }

    [JsonProperty("Content-Type")]
    public string ContentType { get; set; }

    [JsonProperty("Host")]
    public string Host { get; set; }

    [JsonProperty("X-Amzn-Trace-Id")]
    public string XAmznTraceId { get; set; }
}