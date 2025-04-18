using System;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafSendOfferNlpiEventRequest
{
    public string SchemaVersionId { get; set; }
    public string KeySchemaVersionId { get; set; }
    public string BearerToken { get; set; } = string.Empty;
    public SafOfferNlpiEvent EventPayload { get; set; }
}




