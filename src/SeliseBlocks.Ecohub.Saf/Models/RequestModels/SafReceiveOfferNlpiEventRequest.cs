using System;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiveOfferNlpiEventRequest
{
    public required string BearerToken { get; set; } = string.Empty;
    public required string EcohubId { get; set; } = string.Empty;
    public required string PrivateKey { get; set; } = string.Empty;
    public string AutoOffsetReset { get; set; } = string.Empty;

}
