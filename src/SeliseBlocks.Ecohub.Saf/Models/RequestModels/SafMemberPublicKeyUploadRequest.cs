using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafMemberPublicKeyUploadRequest
{
    [Required(ErrorMessage = "BearerToken is required")]
    public string BearerToken { get; set; } = string.Empty;

    public SafMemberPublicKeyUploadRequestPayload Payload { get; set; } = new SafMemberPublicKeyUploadRequestPayload();
}

public class SafMemberPublicKeyUploadRequestPayload
{
    [Required(ErrorMessage = "Version is required")]
    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;

    [Required(ErrorMessage = "Key is required")]
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [Required(ErrorMessage = "ExpireInDays is required")]
    [JsonPropertyName("expireInDays")]
    public string ExpireInDays { get; set; } = string.Empty;
}