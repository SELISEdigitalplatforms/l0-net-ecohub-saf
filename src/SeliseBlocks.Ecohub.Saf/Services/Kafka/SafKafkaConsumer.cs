using Confluent.Kafka;
using Newtonsoft.Json;

namespace SeliseBlocks.Ecohub.Saf.Services.Kafka;

public class SafKafkaConsumer
{
    private readonly string _bootstrapServers;
    private readonly string _topic;
    private readonly string _groupId;
    private readonly string _privateKey;

    public SafKafkaConsumer(string bootstrapServers, string topic, string groupId, string privateKey)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
        _groupId = groupId;
        _privateKey = privateKey;
    }

    public void Receive()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(_topic);

        Console.WriteLine("Listening for messages...");

        try
        {
            while (true)
            {
                var cr = consumer.Consume();
                if (cr?.Message?.Value == null)
                {
                    Console.WriteLine("Received a null message. Skipping...");
                    continue;
                }

                // Replace the problematic line with the following:
                var encryptedEvent = JsonConvert.DeserializeObject<SafOfferNlpiEncryptedEvent>(cr.Message.Value);
                if (encryptedEvent?.Data == null)
                {
                    Console.WriteLine("Received an event with null data. Skipping...");
                    continue;
                }

                var decryptedData = SafCryptoUtils.DecryptAndDecompress(encryptedEvent.Data, _privateKey);
                var domainEvent = encryptedEvent.MapToSafOfferNlpiEvent();
                domainEvent.Data = decryptedData;

                Console.WriteLine($"Received event with Message: {domainEvent.Data.Message}");
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}

