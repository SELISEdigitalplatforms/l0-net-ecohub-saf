namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for managing SAF events. 
/// Provides methods to send and receive offer NLPI events to and from the SAF API.
/// </summary>
public interface ISafRestProxyEventHandler
{
    /// <summary>
    /// Sends an encrypted offer NLPI event to the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing:
    /// <list type="bullet">
    ///   <item><description><see cref="SafSendOfferNlpiEventRequest.SchemaVersionId"/>: Schema version ID for the event (required)</description></item>
    ///   <item><description><see cref="SafSendOfferNlpiEventRequest.KeySchemaVersionId"/>: Schema version ID for the key (required)</description></item>
    ///   <item><description><see cref="SafSendOfferNlpiEventRequest.BearerToken"/>: Authentication token (required)</description></item>
    ///   <item><description><see cref="SafSendOfferNlpiEventRequest.EventPayload"/>: Event data to be encrypted and sent (required)</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafSendOfferNlpiEventResponse"/> containing:
    /// <list type="bullet">
    ///   <item><description><c>IsSuccess</c>: Indicates if the operation succeeded</description></item>
    ///   <item><description><c>Error</c>: Error details if the operation failed</description></item>
    ///   <item><description><c>Data</c>: Response data including:
    ///     <list type="bullet">
    ///       <item><description>KeySchemaId: Schema ID used for the key</description></item>
    ///       <item><description>ValueSchemaId: Schema ID used for the value</description></item>
    ///       <item><description>Offsets: Collection of partition offsets and any errors</description></item>
    ///     </list>
    ///   </description></item>
    /// </list>
    /// </returns>
    Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request);

    /// <summary>
    /// Receives and decrypts offer NLPI events from the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing:
    /// <list type="bullet">
    ///   <item><description><see cref="SafReceiveOfferNlpiEventRequest.BearerToken"/>: Authentication token (required)</description></item>
    ///   <item><description><see cref="SafReceiveOfferNlpiEventRequest.EcohubId"/>: Target Ecohub identifier (required)</description></item>
    ///   <item><description><see cref="SafReceiveOfferNlpiEventRequest.PrivateKey"/>: Private key for decryption (required)</description></item>
    ///   <item><description><see cref="SafReceiveOfferNlpiEventRequest.AutoOffsetReset"/>: Offset reset strategy ("earliest" or "latest")</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafReceiveOfferNlpiEventResponse"/> containing:
    /// <list type="bullet">
    ///   <item><description><c>IsSuccess</c>: Indicates if the operation succeeded</description></item>
    ///   <item><description><c>Error</c>: Error details if the operation failed</description></item>
    ///   <item><description><c>Data</c>: Collection of decrypted <see cref="SafOfferNlpiEvent"/> objects</description></item>
    /// </list>
    /// </returns>
    /// <exception cref="CryptographicException">When decryption fails</exception>
    Task<SafReceiveOfferNlpiEventResponse> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request);
}