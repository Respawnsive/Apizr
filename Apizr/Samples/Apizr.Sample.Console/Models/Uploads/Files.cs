using Newtonsoft.Json;

namespace Apizr.Sample.Console.Models.Uploads;

public class Files
{
    [JsonProperty("streamPart")]
    public string StreamPart { get; set; }
}