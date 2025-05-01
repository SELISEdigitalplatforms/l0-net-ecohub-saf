using System;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafTechUserEnrolmentRequest
{

    [JsonPropertyName("iak")]
    public string Iak { get; set; } = string.Empty;
    [JsonPropertyName("idpUserId")]
    public string IdpUserId { get; set; } = string.Empty;
    [JsonPropertyName("licenceKey")]
    public string LicenceKey { get; set; } = string.Empty;
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = string.Empty;
    [JsonPropertyName("requestTime")]
    public string RequestTime { get; set; } = string.Empty;
    [JsonPropertyName("userAgent")]
    public SafUserAgent UserAgent { get; set; }

}
