using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;

namespace SeliseBlocks.Ecohub.Saf.Services.Kafka;

public class SafKafkaProducer
{
    private const string _privateKey = "-----BEGIN PRIVATE KEY-----MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCymbH9jA2ltGHG1BZDNZcpRh7WOZFpudejM1o8UR6I5d1oTca+3Lz3uyOXceYFucSl28aXQ0tubQnuSVUeuMIBq19OXdQm4mJa6coCGViJO1H97ITqN2i9AwnPF8kdr6RzsgWNo+tRSzs+m+ah3gL/O/bikP4FVHuGMmeozaduvm+pnATioUNdwLoxb4AQPOJJjeoGbwTGiKtercxPpQNeWXBXKMqYFwjv0e00yNVOgdkgij+T9IIs7ek1hGalJYxFnq86suJwGZ5oQWMwYWhSYjOOJCENlKDfu6tPZA8F75huEq4Guag2F4sYe2hRmlF65AKw37eIydWivGQcyaSbAgMBAAECggEAH3HM8yF9459LGbkMhF/Ckes9EaWIEw+7xgmMCROVJzAlV7Bd3gu6H3mszgSpJXfsBfGYWNhpxvLerTvvBx4rViTofkEp0YDJJU2FGfKBcoPlrym9ywjfYWvQBcyfxaC/ePkuXh4ul50BvMexBu2yJGLX2FMDzkduChYExyUSJf6JEYzs9yL6nXrR4Cbmv3716oihyhVpiS1IdH1UMH+R+6tPrFPJoFr86viaR0gN3ejbfAgBDbb4uTPI8aWb8msB7m8yguSutNv+d+xuV5UR4FVR4IKNwblcc021QJIvQHYwMuAzynpwfPhceqQth+vKTlalIlkJh6eC5bDjgyXQgQKBgQDcR+GHk4ABmmka6fTf0t/UPeOYVXELfnVXXMfwDXgFfeY+d8Mvpmf4DIinJo0bL73vMXSqdNybem9DkDQjfB6I3PMh04GhVwPmY/taVqxzLYJg61QVSZ8EXRvFjsS0bDcUgztoVdGOVB+F1RB7n8mK7v0CmM9o75VDlLx8cO63GwKBgQDPj5xINxeLbz7U9dZY97FmLpMpBMDKpLKMHHtzE2Di9YgkeAfKTyz1LX0j72ij97kWzwwQBOx45y01OlFH5YDmEYHalpb6RDBeZVHo22f1vsdB8FWMQMGiqdMlsvfQ1zZU+Ban1yxkZft+b3RfTBgOWb1SzkdEE+hKuGW8ppQggQKBgQCQtHenRHIWm4ToNUCzuCdpma5lZ9t3HX+gAEcnnvF1ShtydeI27y3lePZcN6sCbP5snyRwxYwWZvuoepaFqQe2CM9/LR4/CpZ5Rrzbv4xRrVe0q2L1CQP5LeEMipkVnPEh/IOOKrIauZBrrmfBjlordoumpRO7b4eyeYbIiLeIeQKBgCHgBlmi5CzVkyOem8UZZ9KNd2cSZ4SrLJjBbURyvTVNbVLGZD8YfPXm3q2mvSVFoOegEw/qPc3drPsq8WkSg98IrHDIcwuVZW+CicO/S1BIOq0AVHX3e6LYpKVaeCeVeECV3Ny3uX8JRep0tkF3YdW1v7hsAiWSOi83uSL47OQBAoGAXLkcvaVTxlapU4xwjziE0HEjtFsshmNaXXpaywAgnS88t4lco0cV7MYdkeTlpk01WW/PhrEpldVY+y2W3JimBXEJV0nDz043plwyNj6Y+5zvIbfyXnb3orKNoZ9ft9V5vrkj0bWphCaUVQkkov6s/nSOpaKFyo4MfhPJTSDAJkc=-----END PRIVATE KEY-----";
    private const string _publicKey = "-----BEGIN PUBLIC KEY-----MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAspmx/YwNpbRhxtQWQzWXKUYe1jmRabnXozNaPFEeiOXdaE3Gvty897sjl3HmBbnEpdvGl0NLbm0J7klVHrjCAatfTl3UJuJiWunKAhlYiTtR/eyE6jdovQMJzxfJHa+kc7IFjaPrUUs7Ppvmod4C/zv24pD+BVR7hjJnqM2nbr5vqZwE4qFDXcC6MW+AEDziSY3qBm8ExoirXq3MT6UDXllwVyjKmBcI79HtNMjVToHZIIo/k/SCLO3pNYRmpSWMRZ6vOrLicBmeaEFjMGFoUmIzjiQhDZSg37urT2QPBe+YbhKuBrmoNheLGHtoUZpReuQCsN+3iMnVorxkHMmkmwIDAQAB-----END PUBLIC KEY-----";

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
        string password = "akbz2nXglOtx2JR~";

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
        var encrypted = SafCryptoUtils.CompressAndEncrypt(eventPayload.Data);
        var encryptedEvent = eventPayload.MapToSafOfferNlpiEncryptedEvent();
        encryptedEvent.Data = encrypted;

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
        using var producer = new ProducerBuilder<ProcessIdType, SafOfferNlpiEncryptedEvent>(config)
            .SetValueSerializer(new JsonSerializer<SafOfferNlpiEncryptedEvent>(schemaRegistry, new JsonSerializerConfig
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

        // Create a message to send
        var message = new Message<ProcessIdType, SafOfferNlpiEncryptedEvent>
        {
            Key = new ProcessIdType { ProcessId = Guid.NewGuid() },
            Value = encryptedEvent
        };

        var result = await producer.ProduceAsync(_topic, message);

        Console.WriteLine($"Message sent to {result.TopicPartitionOffset}");
    }
}

