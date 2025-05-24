using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SeliseBlocks.Ecohub.Saf;

public class SafOfferNlpiEncryptedEvent : BaseSafOfferNlpiEvent
{
    [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
    public SafEncryptedData Data { get; set; }
}
public class SafEncryptedData
{
    [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
    public string Payload { get; set; }

    [JsonProperty("md5PayloadHash", NullValueHandling = NullValueHandling.Ignore)]
    public string Md5PayloadHash { get; set; }

    [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
    public List<SafLinks> Links { get; set; }

    [JsonProperty("encryptionKey", NullValueHandling = NullValueHandling.Ignore)]
    public string EncryptionKey { get; set; }

    [JsonProperty("publicKeyVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string PublicKeyVersion { get; set; }

    [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
    public string Message { get; set; }

    [JsonProperty("md5MessageHash", NullValueHandling = NullValueHandling.Ignore)]
    public string Md5MessageHash { get; set; }
}