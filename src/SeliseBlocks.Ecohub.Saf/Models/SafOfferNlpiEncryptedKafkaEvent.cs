using System.Text.Json.Serialization;
using SeliseBlocks.Ecohub.Saf;

namespace SeliseBlocks.Ecohub;

public class SafOfferNlpiEncryptedKafkaEvent
{
    public string id { get; set; }
    public string source { get; set; }
    public string specversion { get; set; }
    public string type { get; set; }
    public string datacontenttype { get; set; }
    public string dataschema { get; set; }
    public string subject { get; set; }
    public string time { get; set; }
    public string licenceKey { get; set; }
    public SafUserKafkaAgent userAgent { get; set; }
    public SafEventKafkaReceiver eventReceiver { get; set; }
    public SafEventKafkaSender eventSender { get; set; }
    public string processId { get; set; }
    public string processStatus { get; set; }
    public string subProcessName { get; set; }
    public string processName { get; set; }
    public string subProcessStatus { get; set; }
    public SafEncryptedKafkaData data { get; set; }
}

public class SafEventKafkaReceiver
{
    public string category { get; set; }

    public string id { get; set; }
}

public class SafUserKafkaAgent
{
    public string name { get; set; }

    public string version { get; set; }
}

public class SafEventKafkaSender
{
    public string category { get; set; }

    public string id { get; set; }
}

public class SafEncryptedKafkaData
{
    public string payload { get; set; }

    //public string md5PayloadHash { get; set; }

    //public List<SafLinks> links { get; set; }

    public string encryptionKey { get; set; }

    public string publicKeyVersion { get; set; }


    //public string message { get; set; }


    //public string md5MessageHash { get; set; }
}
