using System;

namespace SeliseBlocks.Ecohub.Saf;

public interface ISafEventService
{
    /// <summary>
    /// Sends an offer NLPI event to the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the details of the offer NLPI event to be sent.
    /// This includes the event payload, schema version IDs, and authentication details.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafSendOfferNlpiEventResponse"/> object, which includes information about the 
    /// schema IDs and offsets of the sent event.
    /// </returns>
    Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request);

    /// <summary>
    /// Receives an offer NLPI event from the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the details required to receive the offer NLPI event.
    /// This includes the bearer token, Ecohub ID, auto-offset reset configuration, and private key.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafOfferNlpiEvent"/> object, which includes the details of the received event.
    /// </returns>
    Task<SafOfferNlpiEvent> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request);
}
