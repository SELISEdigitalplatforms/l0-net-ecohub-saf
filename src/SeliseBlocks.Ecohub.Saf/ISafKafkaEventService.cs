
namespace SeliseBlocks.Ecohub.Saf;

public interface ISafKafkaEventService
{
    Task<bool> ProduceEventAsync(SafProduceKafkaEventRequest request);
    SafOfferNlpiEvent? ConsumeEventAsync(SafConsumeKafkaEventRequest request);
}
