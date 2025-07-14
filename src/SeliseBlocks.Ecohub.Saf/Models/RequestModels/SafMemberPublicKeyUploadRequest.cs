using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SeliseBlocks.Ecohub.Saf;

public class SafMemberPublicKeyUploadRequest
{
    [Required(ErrorMessage = "BearerToken is required")]
    public string BearerToken { get; set; } = string.Empty;

    public IEnumerable<SafMemberPublicKeyUploadRequestPayload> Payload { get; set; } = [];
}

public class SafMemberPublicKeyUploadRequestPayload
{
    [Required(ErrorMessage = "Version is required")]
    [JsonProperty("version")]
    public string Version { get; set; } = string.Empty;

    [Required(ErrorMessage = "Key is required")]
    [JsonProperty("key")]
    public string Key { get; set; } = string.Empty;

    [Required(ErrorMessage = "keyType is required")]
    [JsonProperty("keyType")]
    public string KeyType { get; set; } = string.Empty;

    [Required(ErrorMessage = "ExpireInDays is required")]
    [JsonProperty("expireInDays")]
    public string ExpireInDays { get; set; } = string.Empty;
}