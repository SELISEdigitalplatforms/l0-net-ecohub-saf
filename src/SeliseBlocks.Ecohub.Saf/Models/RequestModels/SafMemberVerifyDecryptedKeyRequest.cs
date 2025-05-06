using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    [Required(ErrorMessage = "DecryptedContent is required")]
    [JsonPropertyName("decryptedContent")]
    public string DecryptedContent { get; set; } = string.Empty;
}
