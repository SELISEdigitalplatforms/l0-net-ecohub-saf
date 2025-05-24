namespace SeliseBlocks.Ecohub.Saf;

public class SafReceiver
{
    public IEnumerable<string> Idp { get; set; } = new List<string>();
    public string CompanyName { get; set; } = string.Empty;
    public string MemberType { get; set; } = string.Empty;
    public IEnumerable<SafSupportedStandardModel> SafSupportedStandards { get; set; } = new List<SafSupportedStandardModel>();
}

public class SafSupportedStandardModel
{
    public string ProcessName { get; set; } = string.Empty;
    public string ProcessVersion { get; set; } = string.Empty;
}