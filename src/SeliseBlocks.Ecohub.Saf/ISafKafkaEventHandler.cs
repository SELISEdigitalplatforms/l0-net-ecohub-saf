namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for handling Kafka event operations in SAF.
/// Provides methods to produce and consume events using Kafka messaging system.
/// </summary>
public interface ISafKafkaEventHandler
{
    /// <summary>
    /// Asynchronously produces an event to a Kafka topic with encryption and schema validation.
    /// </summary>
    /// <param name="request">
    /// The request object containing:
    /// <list type="bullet">
    ///   <item><description><see cref="SafProduceKafkaEventRequest.KafkaServer"/>: Kafka bootstrap server address (required)</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.KafkaProducerTopic"/>: Target Kafka topic (required)</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.TechUserCertificate"/>: Base64-encoded SSL certificate (required)</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.TechUserPassword"/>: Certificate password (required)</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.SchemaRegistryUrl"/>: Schema Registry URL (required)</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.SchemaRegistryAuth"/>: Schema Registry authentication (required)</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.EventPayload"/>: Event data to be encrypted and sent (required)</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafProduceEventResponse"/> containing:
    /// <list type="bullet">
    ///   <item><description><c>IsSuccess</c>: True if event was produced successfully</description></item>
    ///   <item><description><c>Error</c>: Error details if the operation failed</description></item>
    ///   <item><description><c>Data</c>: Boolean indicating success status</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="CryptographicException">When encryption or certificate operations fail</exception>
    Task<SafProduceEventResponse> ProduceEventAsync(SafProduceKafkaEventRequest request);

    /// <summary>
    /// Consumes and decrypts events from a Kafka topic.
    /// </summary>
    /// <param name="request">
    /// The request object containing:
    /// <list type="bullet">
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.KafkaServer"/>: Kafka bootstrap server address (required)</description></item>
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.TechUserCertificate"/>: Base64-encoded SSL certificate (required)</description></item>
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.TechUserPassword"/>: Certificate password (required)</description></item>
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.GroupId"/>: Consumer group ID (required)</description></item>
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.EcohubId"/>: Ecohub identifier for topic resolution (required)</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafConsumeEventResponse"/> containing:
    /// <list type="bullet">
    ///   <item><description><c>IsSuccess</c>: True if event was consumed successfully</description></item>
    ///   <item><description><c>Error</c>: Error details if the operation failed</description></item>
    ///   <item><description><c>Data</c>: The decrypted <see cref="SafOfferNlpiEvent"/> containing:
    ///     <list type="bullet">
    ///       <item><description>Data.Payload: Decrypted and decompressed event data</description></item>
    ///       <item><description>Data.PublicKey: Associated public key</description></item>
    ///       <item><description>Data.Links: Related resource links</description></item>
    ///     </list>
    ///   </description></item>
    /// </list>
    /// </returns>
    /// <exception cref="CryptographicException">When decryption fails</exception>
    SafConsumeEventResponse ConsumeEvent(SafConsumeKafkaEventRequest request);
}