using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafTechUserEnrolmentRequest
{

    [Required(ErrorMessage = "Iak is required")]
    [JsonPropertyName("iak")]
    public string Iak { get; set; } = string.Empty;

    [Required(ErrorMessage = "IdpUserId is required")]
    [JsonPropertyName("idpUserId")]
    public string IdpUserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "LicenceKey is required")]
    [JsonPropertyName("licenceKey")]
    public string LicenceKey { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password Key is required")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = string.Empty;

    [JsonPropertyName("requestTime")]
    public string RequestTime { get; set; } = string.Empty;

    [JsonPropertyName("userAgent")]
    public SafUserAgent UserAgent { get; set; }

}
