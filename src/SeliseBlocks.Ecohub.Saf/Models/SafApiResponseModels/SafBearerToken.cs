using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafBearerToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("ext_expires_in")]
    public int ExtExpiresIn { get; set; }
}
