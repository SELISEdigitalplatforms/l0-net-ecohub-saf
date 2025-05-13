using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;

namespace SeliseBlocks.Ecohub.Saf.Services.Kafka;

public class SafKafkaProducer
{
    private readonly string _bootstrapServers;
    private readonly string _topic;

    public SafKafkaProducer(string bootstrapServers, string topic)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
    }

    private X509Certificate2 GetClientCertificate()
    {
        // Decode the Base64 string to get the byte array
        string kafkaCertificate = "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";
        byte[] certificateBytes = Convert.FromBase64String(kafkaCertificate);
        string password = "E#5JAYEtn508ds2Y";

        // Create an X509Certificate2 object from the byte array
        X509Certificate2 certificate = new X509Certificate2(
            certificateBytes,
            password,
            X509KeyStorageFlags.Exportable);

        return certificate;
    }

    public class ProcessIdType
    {
        public Guid ProcessId { get; set; }
    }



    public async Task SendAsync(SafOfferNlpiEvent eventPayload)
    {
        var encrypted = SafCryptoUtils.CompressAndEncryptForKafka(eventPayload.Data);
        var encryptedEvent = eventPayload.MapToSafKafkaOfferNlpiEncryptedEvent();
        encryptedEvent.data = encrypted;

        var certificate = GetClientCertificate();  // Implement your logic to get client certificate
        var publicKeyPem = certificate.ExportCertificatePem();
        var privateKey = certificate.GetRSAPrivateKey();
        var privateKeyPem = privateKey.ExportRSAPrivateKeyPem();

        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers,
            SecurityProtocol = SecurityProtocol.Ssl,
            SslCertificatePem = publicKeyPem,
            SslKeyPem = privateKeyPem
        };

        // Schema registry configuration (keep this unchanged or use DB if necessary)
        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            Url = "https://psrc-qrk9d.westeurope.azure.confluent.cloud:443",
            BasicAuthUserInfo = "FCYTB2BG73BWKLZ5:juvZLo3Frvgoqn9Mb5dDJjaXx4NAYf1PwY+k5egoUBEHIYYCnmgzJE/M7uCCYjPv"
        };

        // Parse the JSON input
        var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);

        // Create the Kafka producer
        using var producer = new ProducerBuilder<ProcessIdType, SafOfferNlpiEncryptedKafkaEvent>(config)
            .SetValueSerializer(new JsonSerializer<SafOfferNlpiEncryptedKafkaEvent>(schemaRegistry, new JsonSerializerConfig
            {
                BufferBytes = 100,
                UseLatestVersion = true,
                AutoRegisterSchemas = false,
                SubjectNameStrategy = SubjectNameStrategy.Topic
            }))
            .SetKeySerializer(new JsonSerializer<ProcessIdType>(schemaRegistry, new JsonSerializerConfig
            {
                UseLatestVersion = true,
                AutoRegisterSchemas = false,
            }))
            .Build();
        //using var producer = new ProducerBuilder<string, string>(config).Build();
        // Create a message to send

        var message = new Message<ProcessIdType, SafOfferNlpiEncryptedKafkaEvent>
        {
            Key = new ProcessIdType { ProcessId = Guid.NewGuid() },
            Value = encryptedEvent
        };
        /*
        var message = new Message<string, string>
        {
            Key = "test key",
            Value = "test value"
        };
        */
        var result = await producer.ProduceAsync(_topic, message);

        Console.WriteLine($"Message sent to {result.TopicPartitionOffset}");
    }
}

