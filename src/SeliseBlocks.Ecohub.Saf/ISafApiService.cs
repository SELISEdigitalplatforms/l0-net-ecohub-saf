namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for interacting with the SAF API. 
/// Provides methods to manage member public keys and retrieve receiver information.
/// </summary>
public interface ISafApiService
{
    /// <summary>
    /// Asynchronously retrieves receiver information from the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing:
    /// <list type="bullet">
    ///   <item><description><see cref="SafReceiversRequest.BearerToken"/>: Authentication token (required)</description></item>
    ///   <item><description><see cref="SafReceiversRequestPayload.LicenceKey"/>: The licence key</description></item>
    ///   <item><description><see cref="SafReceiversRequestPayload.Password"/>: The password</description></item>
    ///   <item><description><see cref="SafReceiversRequestPayload.RequestId"/>: Unique request identifier</description></item>
    ///   <item><description><see cref="SafReceiversRequestPayload.RequestTime"/>: Request timestamp</description></item>
    ///   <item><description><see cref="SafReceiversRequestPayload.UserAgent"/>: User agent information</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafReceiversResponse"/> containing:
    /// <list type="bullet">
    ///   <item><description><c>IsSuccess</c>: Operation status</description></item>
    ///   <item><description><c>Error</c>: Error details if failed</description></item>
    ///   <item><description><c>Data</c>: Collection of <see cref="SafReceiver"/> objects</description></item>
    /// </list>
    /// </returns>
    Task<SafReceiversResponse> GetReceiversAsync(SafReceiversRequest request);


}