using SeliseBlocks.Ecohub.Saf.Models;

namespace SeliseBlocks.Ecohub.Saf;

public interface ISafKafkaEventService
{
    Task<bool> ProduceEventAsync(SafOfferNlpiKafkaEvent eventPayload);
    Task ConsumeEventAsync(SafOfferNlpiConsumeKafkaEvent eventPayload);
}
