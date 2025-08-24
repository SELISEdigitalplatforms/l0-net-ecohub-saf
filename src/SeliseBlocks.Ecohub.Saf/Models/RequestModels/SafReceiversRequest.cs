using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiversRequest
{
    [Required(ErrorMessage = "BearerToken is required")]
    public string BearerToken { get; set; } = string.Empty;
    public SafReceiversRequestPayload Payload { get; set; } = new SafReceiversRequestPayload();

}
public class SafReceiversRequestPayload
{
    [Required(ErrorMessage = "LicenceKey is required")]
    [JsonProperty("licenceKey")]
    public string LicenceKey { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [JsonProperty("password")]
    public string Password { get; set; } = string.Empty;

    [JsonProperty("requestId")]
    public string RequestId { get; set; } = string.Empty;
    [JsonProperty("requestTime")]
    public string RequestTime { get; set; } = string.Empty;

    [JsonProperty("userAgent")]
    public SafUserAgent UserAgent { get; set; }

}
