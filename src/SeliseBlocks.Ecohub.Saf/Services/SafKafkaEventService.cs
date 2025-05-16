using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;
using SeliseBlocks.Ecohub.Saf.Helpers;
using System.Security.Cryptography.X509Certificates;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class SafKafkaEventHandler : ISafKafkaEventHandler
{
    public async Task<bool> ProduceEventAsync(SafProduceKafkaEventRequest request)
    {
        SetDefaultValue(request);
        try
        {
            var encrypted = SafEventDataResolver.CompressAndEncryptForKafka(request.EventPayload.Data);
            var encryptedEvent = request.EventPayload.MapToSafKafkaOfferNlpiEncryptedEvent();
            encryptedEvent.data = encrypted;

            var certificate = GetTechUserCertificate(request.TechUserCertificate, request.TechUserPassword);
            var publicKeyPem = certificate.ExportCertificatePem();
            var privateKey = certificate.GetRSAPrivateKey();
            var privateKeyPem = privateKey.ExportRSAPrivateKeyPem();

            var config = new ProducerConfig
            {
                BootstrapServers = request.KafkaServer,
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCertificatePem = publicKeyPem,
                SslKeyPem = privateKeyPem
            };

            // Schema registry configuration (keep this unchanged or use DB if necessary)
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = request.SchemaRegistryUrl,
                BasicAuthUserInfo = request.SchemaRegistryAuth
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

            var result = await producer.ProduceAsync(request.KafkaProducerTopic, message);

            Console.WriteLine($"Message sent to {result.TopicPartitionOffset}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error producing Kafka event: {ex.Message}");
            return false;
        }
    }

    public SafOfferNlpiEvent? ConsumeEvent(SafConsumeKafkaEventRequest request)
    {
        var consumerTopic = SafDriverConstant.KafkaConsumerTopic.Replace("{ecohubId}", request.EcohubId); ;
        SafOfferNlpiEvent? response = null;
        try
        {
            var certificate = GetTechUserCertificate(request.TechUserCertificate, request.TechUserPassword);
            var privateKey = certificate.GetRSAPrivateKey();
            var privateKeyPem = privateKey.ExportRSAPrivateKeyPem();

            var config = new ConsumerConfig
            {
                BootstrapServers = request.KafkaServer,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCertificatePem = certificate.ExportCertificatePem(),
                SslKeyPem = privateKeyPem,
                GroupId = request.GroupId,
                EnableAutoCommit = false
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(consumerTopic);

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

                            response = ExtractConsumedData(consumerResult.Message.Value, privateKeyPem);
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
        return response;
    }


    private SafOfferNlpiEvent ExtractConsumedData(string jsonData, string privateKey)
    {
        var eventResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<SafReceiveOfferNlpiEventResponse>(jsonData);
        var safData = eventResponse.Value.MapToSafOfferNlpiEvent();
        safData.Data = SafEventDataResolver.DecryptAndDecompress(eventResponse.Value.Data, privateKey);
        return safData;
        // Process the decrypted and decompressed data as needed
    }
    private void SetDefaultValue(SafProduceKafkaEventRequest request)
    {
        request.KafkaProducerTopic = string.IsNullOrWhiteSpace(request.KafkaProducerTopic)
        ? SafDriverConstant.KafkaProducerTopic : request.KafkaProducerTopic;

        request.SchemaRegistryUrl = string.IsNullOrWhiteSpace(request.SchemaRegistryUrl)
        ? SafDriverConstant.SchemaRegistryUrl : request.SchemaRegistryUrl;

        request.SchemaRegistryAuth = string.IsNullOrWhiteSpace(request.SchemaRegistryAuth)
        ? SafDriverConstant.SchemaRegistryAuth : request.SchemaRegistryAuth;

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


    private class ProcessIdType
    {
        public Guid ProcessId { get; set; }
    }

}
