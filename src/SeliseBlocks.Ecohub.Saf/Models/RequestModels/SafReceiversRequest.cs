using System;

namespace SeliseBlocks.Ecohub.Saf.Models.RequestModels;

public class SafReceiversRequest
{
    public string BearerToken { get; set; } = string.Empty;
    public SafReceiversRequestPayload Payload { get; set; } = new SafReceiversRequestPayload();

}
public class SafReceiversRequestPayload
{
    public string LicenceKey { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string RequestId { get; set; } = string.Empty;
    public Object? UserAgent { get; set; }

}
