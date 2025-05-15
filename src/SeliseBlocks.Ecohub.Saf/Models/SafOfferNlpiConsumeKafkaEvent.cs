namespace SeliseBlocks.Ecohub.Saf.Models;

public class SafOfferNlpiConsumeKafkaEvent
{
    public string KafkaServer { get; set; }
    public string KafkaTopic { get; set; }
    public string TechUserCertificate { get; set; }
    public string TechUserPassword { get; set; }
    public string TechUserGroupId { get; set; }
}
