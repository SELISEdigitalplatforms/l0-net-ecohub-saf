using System.Text.Json.Serialization;
using SeliseBlocks.Ecohub.Saf;

namespace SeliseBlocks.Ecohub;

public class SafOfferNlpiEncryptedKafkaEvent
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string id { get; set; }

    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string source { get; set; }

    [JsonPropertyName("specversion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string specversion { get; set; }

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string type { get; set; }

    [JsonPropertyName("datacontenttype")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string datacontenttype { get; set; }

    [JsonPropertyName("dataschema")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string dataschema { get; set; }

    [JsonPropertyName("subject")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string subject { get; set; }

    [JsonPropertyName("time")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string time { get; set; }

    [JsonPropertyName("licenceKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string licenceKey { get; set; }

    [JsonPropertyName("userAgent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SafUserKafkaAgent userAgent { get; set; }

    [JsonPropertyName("eventReceiver")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SafEventKafkaReceiver eventReceiver { get; set; }

    [JsonPropertyName("eventSender")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SafEventKafkaSender eventSender { get; set; }

    [JsonPropertyName("processId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string processId { get; set; }

    [JsonPropertyName("processStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string processStatus { get; set; }

    [JsonPropertyName("subProcessName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string subProcessName { get; set; }

    [JsonPropertyName("processName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string processName { get; set; }

    [JsonPropertyName("subProcessStatus")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string subProcessStatus { get; set; }

    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SafEncryptedKafkaData data { get; set; }
}

public class SafEventKafkaReceiver
{
    [JsonPropertyName("category")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string category { get; set; }

    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string id { get; set; }
}

public class SafUserKafkaAgent
{
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string name { get; set; }

    [JsonPropertyName("version")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string version { get; set; }
}

public class SafEventKafkaSender
{
    [JsonPropertyName("category")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string category { get; set; }

    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string id { get; set; }
}

public class SafEncryptedKafkaData
{
    [JsonPropertyName("payload")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string payload { get; set; }

    [JsonPropertyName("md5PayloadHash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string md5PayloadHash { get; set; }

    [JsonPropertyName("links")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<SafLinks> links { get; set; }

    [JsonPropertyName("encryptionKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string encryptionKey { get; set; }

    [JsonPropertyName("publicKeyVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string publicKeyVersion { get; set; }

    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string message { get; set; }

    [JsonPropertyName("md5MessageHash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string md5MessageHash { get; set; }
}
