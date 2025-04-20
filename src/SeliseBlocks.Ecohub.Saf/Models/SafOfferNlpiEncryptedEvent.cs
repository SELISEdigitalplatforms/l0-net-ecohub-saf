using System;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

internal class SafOfferNlpiEncryptedEvent : BaseSafOfferNlpiEvent
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SafEncryptedData Data { get; set; }

    // [JsonPropertyName("dataBase64")]
    // [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // public string DataBase64 { get; set; }
}
internal class SafEncryptedData
{
    [JsonPropertyName("payload")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Payload { get; set; }

    [JsonPropertyName("md5PayloadHash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Md5PayloadHash { get; set; }

    [JsonPropertyName("links")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<SafLinks> Links { get; set; }

    [JsonPropertyName("encryptionKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string EncryptionKey { get; set; }

    [JsonPropertyName("publicKeyVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string PublicKeyVersion { get; set; }

    [JsonPropertyName("message")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; set; }

    [JsonPropertyName("md5MessageHash")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Md5MessageHash { get; set; }
}