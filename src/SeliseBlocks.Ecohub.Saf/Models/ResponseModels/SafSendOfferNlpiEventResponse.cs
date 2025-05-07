using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafSendOfferNlpiEventResponse
{
    [JsonPropertyName("key_schema_id")]
    public int KeySchemaId { get; set; }
    [JsonPropertyName("value_schema_id")]
    public int ValueSchemaId { get; set; }
    [JsonPropertyName("offsets")]
    public IEnumerable<SafSendOfferNlpiEventResponseOffset> Offsets { get; set; } = [];
}

public class SafSendOfferNlpiEventResponseOffset
{
    [JsonPropertyName("partition")]
    public int Partition { get; set; }
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
    [JsonPropertyName("error_code")]
    public int? ErrorCode { get; set; }
    [JsonPropertyName("error")]
    public string? Error { get; set; }
}
