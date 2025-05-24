namespace SeliseBlocks.Ecohub.Saf;

public class SafMemberPublicKey
{
    public string KeyId { get; set; }
    public string MembershipId { get; set; }
    public string Version { get; set; }
    public string Key { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public DateTime ActivatedAt { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string EcoHubStatus { get; set; }
}
