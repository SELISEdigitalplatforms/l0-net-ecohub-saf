using System;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiversRequest
{
    public string BearerToken { get; set; } = string.Empty;
    public SafReceiversRequestPayload Payload { get; set; } = new SafReceiversRequestPayload();

}
public class SafReceiversRequestPayload
{

    [JsonPropertyName("licenceKey")]
    public string LicenceKey { get; set; } = string.Empty;
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = string.Empty;
    [JsonPropertyName("requestTime")]
    public string RequestTime { get; set; } = string.Empty;

    [JsonPropertyName("UserAgent")]
    public SafUserAgent UserAgent { get; set; }

}
