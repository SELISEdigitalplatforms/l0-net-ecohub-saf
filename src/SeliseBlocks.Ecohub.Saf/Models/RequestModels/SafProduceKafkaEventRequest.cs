using System;
using System.ComponentModel.DataAnnotations;

namespace SeliseBlocks.Ecohub.Saf;

public class SafProduceKafkaEventRequest
{
    [Required(ErrorMessage = "KafkaServer is required")]
    public string KafkaServer { get; set; }
    [Required(ErrorMessage = "KafkaProducerTopic is required")]
    public string KafkaProducerTopic { get; set; }
    [Required(ErrorMessage = "TechUserCertificate is required")]
    public string TechUserCertificate { get; set; }
    [Required(ErrorMessage = "TechUserPassword is required")]
    public string TechUserPassword { get; set; }
    [Required(ErrorMessage = "SchemaRegistryUrl is required")]
    public string SchemaRegistryUrl { get; set; }
    [Required(ErrorMessage = "SchemaRegistryAuth is required")]
    public string SchemaRegistryAuth { get; set; }

    [Required(ErrorMessage = "EventPayload is required")]
    public SafOfferNlpiEvent EventPayload { get; set; }
}
