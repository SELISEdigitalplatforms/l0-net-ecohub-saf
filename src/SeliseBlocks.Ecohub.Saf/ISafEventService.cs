using System;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for managing SAF events. 
/// Provides methods to send and receive offer NLPI events to and from the SAF API.
/// </summary>
public interface ISafEventService
{
    /// <summary>
    /// Sends an offer NLPI event to the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the details of the offer NLPI event to be sent.
    /// This includes:
    /// - <see cref="SafSendOfferNlpiEventRequest.SchemaVersionId"/>: The schema version ID for the event payload.
    /// - <see cref="SafSendOfferNlpiEventRequest.KeySchemaVersionId"/>: The schema version ID for the key.
    /// - <see cref="SafSendOfferNlpiEventRequest.BearerToken"/>: The bearer token for authentication.
    /// - <see cref="SafSendOfferNlpiEventRequest.EventPayload"/>: The event payload to be sent.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafSendOfferNlpiEventResponse"/> object, which includes information about the 
    /// schema IDs and offsets of the sent event.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue with the HTTP request, such as a network error or invalid response.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if there is an issue serializing or deserializing the request or response.
    /// </exception>    
    Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request);

    /// <summary>
    /// Asynchronously receives an offer NLPI event from the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the details required to receive the offer NLPI event.
    /// This includes:
    /// - <see cref="SafReceiveOfferNlpiEventRequest.BearerToken"/>: The bearer token for authentication.
    /// - <see cref="SafReceiveOfferNlpiEventRequest.EcohubId"/>: The Ecohub ID to identify the target Ecohub.
    /// - <see cref="SafReceiveOfferNlpiEventRequest.AutoOffsetReset"/>: The auto-offset reset configuration (e.g., "earliest" or "latest").
    /// - <see cref="SafReceiveOfferNlpiEventRequest.PrivateKey"/>: The private key for decryption or authentication purposes.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of 
    /// <see cref="SafOfferNlpiEvent"/> objects, which include the details of the received events.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue with the HTTP request, such as a network error or invalid response.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if there is an issue deserializing the response from the SAF API.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the bearer token is invalid or expired.
    /// </exception>
    Task<IEnumerable<SafOfferNlpiEvent>> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request);
}
