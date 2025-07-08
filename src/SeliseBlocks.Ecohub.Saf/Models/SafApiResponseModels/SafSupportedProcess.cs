using System.Text.Json.Serialization;

namespace SeliseBlocks.Ecohub.Saf;

public class SafSupportedProcess
{
    [JsonPropertyName("ProcessName")]
    public string ProcessName { get; set; } = string.Empty;
    [JsonPropertyName("ProcessVersion")]
    public string ProcessVersion { get; set; } = string.Empty;
}
