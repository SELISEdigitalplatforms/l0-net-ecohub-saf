using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;
using SeliseBlocks.Ecohub.Saf.Services.Kafka;
using System.Security.Cryptography.X509Certificates;
using SeliseBlocks.Ecohub.Saf.Models;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class SafKafkaEventService : ISafKafkaEventService
{
    private class ProcessIdType
    {
        public Guid ProcessId { get; set; }
    }

    private static X509Certificate2 GetTechUserCertificate(string techUserCertificate, string password)
    {
        byte[] certificateBytes = Convert.FromBase64String(techUserCertificate);

        X509Certificate2 certificate = new X509Certificate2(
            certificateBytes,
            password,
            X509KeyStorageFlags.Exportable);

        return certificate;
    }

    public async Task<bool> ProduceEventAsync(SafOfferNlpiKafkaEvent eventPayload)
    {
        try 
        {
            var encrypted = SafCryptoUtils.CompressAndEncryptForKafka(eventPayload.Data);
            var encryptedEvent = eventPayload.MapToSafKafkaOfferNlpiEncryptedEvent();
            encryptedEvent.data = encrypted;

            var certificate = GetTechUserCertificate(eventPayload.TechUserCertificate, eventPayload.TechUserPassword);
            var publicKeyPem = certificate.ExportCertificatePem();
            var privateKey = certificate.GetRSAPrivateKey();
            var privateKeyPem = privateKey.ExportRSAPrivateKeyPem();

            var config = new ProducerConfig
            {
                BootstrapServers = eventPayload.KafkaServer,
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCertificatePem = publicKeyPem,
                SslKeyPem = privateKeyPem
            };

            // Schema registry configuration (keep this unchanged or use DB if necessary)
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = eventPayload.SchemaRegistryUrl,
                BasicAuthUserInfo = eventPayload.SchemaRegistryAuth
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

            var message = new Message<ProcessIdType, SafOfferNlpiEncryptedKafkaEvent>
            {
                Key = new ProcessIdType { ProcessId = Guid.NewGuid() },
                Value = encryptedEvent
            };

            var result = await producer.ProduceAsync(eventPayload.KafkaTopic, message);

            Console.WriteLine($"Message sent to {result.TopicPartitionOffset}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error producing Kafka event: {ex.Message}");
            return false;
        }
    }

    public async Task ConsumeEventAsync(SafOfferNlpiConsumeKafkaEvent eventPayload)
    {
        try
        {
            var certificate = GetTechUserCertificate(eventPayload.TechUserCertificate, eventPayload.TechUserPassword);
            var privateKey = certificate.GetRSAPrivateKey();

            var config = new ConsumerConfig
            {
                BootstrapServers = eventPayload.KafkaServer,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCertificatePem = certificate.ExportCertificatePem(),
                SslKeyPem = privateKey.ExportRSAPrivateKeyPem(),
                GroupId = eventPayload.TechUserGroupId,
                EnableAutoCommit = false
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(eventPayload.KafkaTopic);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        try
                        {
                            var consumerResult = consumer.Consume(cts.Token);
                        }
                        catch (ConsumeException ex)
                        {
                            Console.WriteLine($"Kafka consume error: {ex.Error.Reason}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during message consumption: {ex.Message}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Consumer loop has been canceled.");
                }
                finally
                {
                    consumer.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Kafka consumer: {ex.Message}");
        }
    }
}
