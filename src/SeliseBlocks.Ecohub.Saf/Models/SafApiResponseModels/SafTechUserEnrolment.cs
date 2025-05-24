using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafTechUserEnrolment
{
    [JsonPropertyName("techUserCert")]
    public string TechUserCert { get; set; } = string.Empty;
    [JsonPropertyName("oAuth2")]
    public SafTechUserEnrolmentOauth2 OAuth2 { get; set; }

}
public class SafTechUserEnrolmentOauth2
{
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; } = string.Empty;
    [JsonPropertyName("clientSecret")]
    public string ClientSecret { get; set; } = string.Empty;
    [JsonPropertyName("openIdConfigurationEndpoint")]
    public string OpenIdConfigurationEndpoint { get; set; } = string.Empty;
}
