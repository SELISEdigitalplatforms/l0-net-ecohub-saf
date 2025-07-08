namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiver
{
    public IEnumerable<string> Idp { get; set; } = [];
    public string CompanyName { get; set; } = string.Empty;
    public string MemberType { get; set; } = string.Empty;
    public IEnumerable<SafSupportedProcess> SupportedProcesses { get; set; } = [];
}
