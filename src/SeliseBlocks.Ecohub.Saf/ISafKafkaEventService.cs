
namespace SeliseBlocks.Ecohub.Saf;

public interface ISafKafkaEventService
{
    /// <summary>
    /// Asynchronously produces an event to a Kafka topic.
    /// This method encrypts and serializes the event payload, prepares the Kafka producer configuration,
    /// and sends the message to the specified Kafka topic.
    /// </summary>
    /// <param name="request">
    /// The request object containing all necessary information for producing the event, including:
    /// <list type="bullet">
    ///   <item><description><see cref="SafProduceKafkaEventRequest.KafkaServer"/>: The Kafka bootstrap server address.</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.KafkaProducerTopic"/>: The Kafka topic to which the event will be produced.</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.TechUserCertificate"/>: The base64-encoded certificate for authentication.</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.TechUserPassword"/>: The password for the certificate.</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.SchemaRegistryUrl"/>: The URL of the schema registry.</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.SchemaRegistryAuth"/>: The authentication information for the schema registry.</description></item>
    ///   <item><description><see cref="SafProduceKafkaEventRequest.EventPayload"/>: The event payload to be sent.</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean value:
    /// "true" if the event was produced successfully; otherwise, "false".
    /// </returns>
    Task<bool> ProduceEventAsync(SafProduceKafkaEventRequest request);

    /// <summary>
    /// Consumes an event from a Kafka topic and returns the deserialized event payload.
    /// This method connects to the specified Kafka topic, consumes a message, decrypts and deserializes the payload,
    /// and returns the resulting <see cref="SafOfferNlpiEvent"/> object.
    /// </summary>
    /// <param name="request">
    /// The request object containing all necessary information for consuming the event, including:
    /// <list type="bullet">
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.KafkaServer"/>: The Kafka bootstrap server address.</description></item>
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.TechUserCertificate"/>: The base64-encoded certificate for authentication.</description></item>
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.TechUserPassword"/>: The password for the certificate.</description></item>
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.GroupId"/>: The Kafka consumer group ID.</description></item>
    ///   <item><description><see cref="SafConsumeKafkaEventRequest.EcohubId"/>: The Ecohub identifier used for topic resolution.</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// The deserialized <see cref="SafOfferNlpiEvent"/> if a message is successfully consumed and processed; otherwise, null.
    /// </returns>
    SafOfferNlpiEvent? ConsumeEventAsync(SafConsumeKafkaEventRequest request);
}
