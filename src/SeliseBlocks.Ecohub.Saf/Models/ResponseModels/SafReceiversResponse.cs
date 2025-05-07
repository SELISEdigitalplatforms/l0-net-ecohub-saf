namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiversResponse
{
    public IEnumerable<string> Idp { get; set; } = new List<string>();
    public string CompanyName { get; set; } = string.Empty;
    public string MemberType { get; set; } = string.Empty;
    public IEnumerable<SafSupportedStandard> SafSupportedStandards { get; set; } = new List<SafSupportedStandard>();
}

public class SafSupportedStandard
{
    public string ProcessName { get; set; } = string.Empty;
    public string ProcessVersion { get; set; } = string.Empty;
}