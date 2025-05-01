using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafSendOfferNlpiEventRequest
{
    [Required(ErrorMessage = "SchemaVersionId is required")]
    public string SchemaVersionId { get; set; }

    [Required(ErrorMessage = "KeySchemaVersionId is required")]
    public string KeySchemaVersionId { get; set; }

    [Required(ErrorMessage = "BearerToken is required")]
    public string BearerToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "EventPayload is required")]
    public SafOfferNlpiEvent EventPayload { get; set; }
}




