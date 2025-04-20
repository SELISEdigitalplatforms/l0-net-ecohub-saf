using System;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiveOfferNlpiEventRequest
{
    public string BearerToken { get; set; } = string.Empty;
    public string EcohubId { get; set; } = string.Empty;
    public string AutoOffsetReset { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;

}
