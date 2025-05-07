using System.ComponentModel.DataAnnotations;

namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiveOfferNlpiEventRequest
{

    [Required(ErrorMessage = "BearerToken is required")]
    public string BearerToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "EcohubId is required")]
    public string EcohubId { get; set; } = string.Empty;

    [Required(ErrorMessage = "PrivateKey is required")]
    public string PrivateKey { get; set; } = string.Empty;

    public string AutoOffsetReset { get; set; } = string.Empty;

}
