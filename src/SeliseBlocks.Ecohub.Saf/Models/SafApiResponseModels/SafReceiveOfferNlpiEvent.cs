using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiveOfferNlpiEvent
{
    [JsonPropertyName("topic")]
    public string Topic { get; set; }
    [JsonPropertyName("key")]
    public SafReceiveOfferNlpiEventKey Key { get; set; }
    [JsonPropertyName("partition")]
    public int Partition { get; set; }
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    [JsonPropertyName("value")]
    public SafOfferNlpiEncryptedEvent Value { get; set; }
}

public class SafReceiveOfferNlpiEventKey
{
    public string ProcessId { get; set; }
}
