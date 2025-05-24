using System.ComponentModel.DataAnnotations;

namespace SeliseBlocks.Ecohub.Saf;

public class SafConsumeKafkaEventRequest
{
    [Required(ErrorMessage = "KafkaServer is required")]
    public string KafkaServer { get; set; }
    [Required(ErrorMessage = "TechUserCertificate is required")]
    public string TechUserCertificate { get; set; }
    [Required(ErrorMessage = "TechUserPassword is required")]
    public string TechUserPassword { get; set; }
    [Required(ErrorMessage = "GroupId is required")]
    public string GroupId { get; set; }
    [Required(ErrorMessage = "EcohubId is required")]
    public string EcohubId { get; set; }
}
