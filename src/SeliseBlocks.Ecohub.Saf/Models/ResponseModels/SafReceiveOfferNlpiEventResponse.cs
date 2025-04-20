using System;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiveOfferNlpiEventResponse
{
    [JsonPropertyName("topic")]
    public string Topic { get; set; }
    [JsonPropertyName("key")]
    public string Key { get; set; }
    [JsonPropertyName("partition")]
    public int Partition { get; set; }
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    [JsonPropertyName("value")]
    public SafOfferNlpiEvent Value { get; set; }
}
