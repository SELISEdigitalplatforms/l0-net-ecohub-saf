using Newtonsoft.Json;

namespace SeliseBlocks.Ecohub.Saf;

public class BaseSafOfferNlpiEvent
{

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
    public string Source { get; set; }

    [JsonProperty("specversion", NullValueHandling = NullValueHandling.Ignore)]
    public string Specversion { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("datacontenttype", NullValueHandling = NullValueHandling.Ignore)]
    public string DataContentType { get; set; }

    [JsonProperty("dataschema", NullValueHandling = NullValueHandling.Ignore)]
    public string DataSchema { get; set; }

    [JsonProperty("subject", NullValueHandling = NullValueHandling.Ignore)]
    public string Subject { get; set; }

    [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
    public string Time { get; set; }

    [JsonProperty("licenceKey", NullValueHandling = NullValueHandling.Ignore)]
    public string LicenceKey { get; set; }

    [JsonProperty("userAgent", NullValueHandling = NullValueHandling.Ignore)]
    public SafUserAgent UserAgent { get; set; }

    [JsonProperty("eventReceiver", NullValueHandling = NullValueHandling.Ignore)]
    public SafEventReceiver EventReceiver { get; set; }

    [JsonProperty("eventSender", NullValueHandling = NullValueHandling.Ignore)]
    public SafEventSender EventSender { get; set; }

    [JsonProperty("processId", NullValueHandling = NullValueHandling.Ignore)]
    public string ProcessId { get; set; }

    [JsonProperty("processStatus", NullValueHandling = NullValueHandling.Ignore)]
    public string ProcessStatus { get; set; }

    [JsonProperty("subProcessName", NullValueHandling = NullValueHandling.Ignore)]
    public string SubProcessName { get; set; }

    [JsonProperty("processName", NullValueHandling = NullValueHandling.Ignore)]
    public string ProcessName { get; set; }
    [JsonProperty("processVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string ProcessVersion { get; set; }

    [JsonProperty("subProcessStatus", NullValueHandling = NullValueHandling.Ignore)]
    public string SubProcessStatus { get; set; }
}
public class SafLink
{
    [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
    public string Href { get; set; }

    [JsonProperty("rel", NullValueHandling = NullValueHandling.Ignore)]
    public string Rel { get; set; }

    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; set; }
}

public class SafUserAgent
{
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; set; }

    [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
    public string Version { get; set; }
}

public class SafEventReceiver
{
    [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
    public string Category { get; set; }

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }
}

public class SafEventSender
{
    [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
    public string Category { get; set; }

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }
}
