using System;

namespace SeliseBlocks.Ecohub.Saf;

public interface ISafEventService
{
    Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request);

    Task<SafOfferNlpiEvent> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request);
}
