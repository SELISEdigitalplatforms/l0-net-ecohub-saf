using SeliseBlocks.Ecohub.Saf.Models;

namespace SeliseBlocks.Ecohub.Saf;

public interface ISafKafkaEventService
{
    Task ProduceEventAsync(SafOfferNlpiKafkaEvent eventPayload);
    Task ConsumeEventAsync(SafOfferNlpiConsumeKafkaEvent eventPayload);
}
