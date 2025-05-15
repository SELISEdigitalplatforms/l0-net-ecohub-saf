
namespace SeliseBlocks.Ecohub.Saf.Models;

public class SafOfferNlpiKafkaEvent : SafOfferNlpiEvent
{
    public string KafkaServer { get; set; }
    public string KafkaTopic { get; set; }
    public string TechUserCertificate { get; set; }
    public string TechUserPassword { get; set; }
    public string SchemaRegistryUrl { get; set; }
    public string SchemaRegistryAuth { get; set; }

}
