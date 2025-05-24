namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for handling authentication with the SAF API.
/// Provides methods to obtain bearer tokens, enroll technical users, and retrieve OpenID configurations.
/// </summary>
public interface ISafAuthService
{
    /// <summary>
    /// Asynchronously retrieves a bearer token from the SAF API using the provided credentials.
    /// </summary>
    /// <param name="request">
    /// The request object containing:
    /// <list type="bullet">
    ///   <item><description><c>RequestUrl</c>: The endpoint URL for obtaining the bearer token.</description></item>
    ///   <item><description><c>Body</c>: The request body containing:</description>
    ///     <list type="bullet">
    ///       <item><description><see cref="SafAccessTokenRequestBody.GrantType"/>: The OAuth2 grant type (required).</description></item>
    ///       <item><description><see cref="SafAccessTokenRequestBody.ClientId"/>: The client ID for authentication (required).</description></item>
    ///       <item><description><see cref="SafAccessTokenRequestBody.ClientSecret"/>: The client secret for authentication (required).</description></item>
    ///       <item><description><see cref="SafAccessTokenRequestBody.Scope"/>: The requested scope (optional).</description></item>
    ///     </list>
    ///   </item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafBearerTokenResponse"/> containing:
    /// <list type="bullet">
    ///   <item><description><c>IsSuccess</c>: Indicates if the request was successful.</description></item>
    ///   <item><description><c>Error</c>: Error details if the request failed.</description></item>
    ///   <item><description><c>Data</c>: The bearer token data including access token, token type, and expiration.</description></item>
    /// </list>
    /// </returns>
    Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request);

    /// <summary>
    /// Asynchronously enrolls a technical user in the SAF system.
    /// </summary>
    /// <param name="request">
    /// The enrollment request containing:
    /// <list type="bullet">
    ///   <item><description><see cref="SafTechUserEnrolmentRequest.Iak"/>: The IAK identifier (required).</description></item>
    ///   <item><description><see cref="SafTechUserEnrolmentRequest.IdpUserId"/>: The IDP user identifier (required).</description></item>
    ///   <item><description><see cref="SafTechUserEnrolmentRequest.LicenceKey"/>: The license key (required).</description></item>
    ///   <item><description><see cref="SafTechUserEnrolmentRequest.Password"/>: The user's password (required).</description></item>
    ///   <item><description><see cref="SafTechUserEnrolmentRequest.RequestId"/>: A unique request identifier.</description></item>
    ///   <item><description><see cref="SafTechUserEnrolmentRequest.RequestTime"/>: The timestamp of the request.</description></item>
    ///   <item><description><see cref="SafTechUserEnrolmentRequest.UserAgent"/>: Information about the user agent.</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// A <see cref="SafTechUserEnrolmentResponse"/> containing:
    /// <list type="bullet">
    ///   <item><description><c>IsSuccess</c>: Indicates if the enrollment was successful.</description></item>
    ///   <item><description><c>Error</c>: Error details if the enrollment failed.</description></item>
    ///   <item><description><c>Data</c>: The enrollment data including technical user certificate and OAuth2 credentials.</description></item>
    /// </list>
    /// </returns>
    Task<SafTechUserEnrolmentResponse> EnrolTechUserAsync(SafTechUserEnrolmentRequest request);

    /// <summary>
    /// Asynchronously retrieves the OpenID configuration from the specified URL.
    /// </summary>
    /// <param name="openIdUrl">The URL of the OpenID configuration endpoint.</param>
    /// <returns>
    /// A <see cref="SafOpenIdConfigurationResponse"/> containing:
    /// <list type="bullet">
    ///   <item><description><c>IsSuccess</c>: Indicates if the request was successful.</description></item>
    ///   <item><description><c>Error</c>: Error details if the request failed.</description></item>
    ///   <item><description><c>Data</c>: The OpenID configuration including endpoints, supported methods, and claims.</description></item>
    /// </list>
    /// </returns>
    Task<SafOpenIdConfigurationResponse> GetOpenIdConfigurationAsync(Uri openIdUrl);
}