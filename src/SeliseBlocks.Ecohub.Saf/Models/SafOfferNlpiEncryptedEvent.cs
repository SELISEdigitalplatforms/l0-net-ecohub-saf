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

    [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
    public List<SafLink> Links { get; set; }

    [JsonProperty("encryptionKey", NullValueHandling = NullValueHandling.Ignore)]
    public string EncryptionKey { get; set; }

    [JsonProperty("publicKeyVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string PublicKeyVersion { get; set; }

    [JsonProperty("payloadSignature", NullValueHandling = NullValueHandling.Ignore)]
    public string PayloadSignature { get; set; }
    [JsonProperty("signatureKeyVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string SignatureKeyVersion { get; set; }

}