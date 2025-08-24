using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SeliseBlocks.Ecohub.Saf;

public class SafMemberVerifyDecryptedKeyRequest
{
    [Required(ErrorMessage = "BearerToken is required")]
    public string BearerToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "KeyId is required")]
    public string KeyId { get; set; } = string.Empty;

    public SafMemberVerifyDecryptedKeyRequestPayload Payload { get; set; } = new SafMemberVerifyDecryptedKeyRequestPayload();
}

public class SafMemberVerifyDecryptedKeyRequestPayload
{
    [Required(ErrorMessage = "VerifiedContent is required")]
    [JsonProperty("verifiedContent")]
    public string VerifiedContent { get; set; } = string.Empty;
}
