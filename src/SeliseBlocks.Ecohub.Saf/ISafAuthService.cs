using System;
using SeliseBlocks.Ecohub.Saf;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for handling authentication with the SAF API.
/// Provides methods to obtain bearer tokens, techUserEnrolment, and retrieve OpenID configurations.
/// </summary>
public interface ISafAuthService
{
    /// <summary>
    /// Asynchronously retrieves a bearer token from the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the URL and body for obtaining the bearer token.
    /// The body includes:
    /// - <see cref="SafAccessTokenRequestBody.GrantType"/>: The grant type for the token request.
    /// - <see cref="SafAccessTokenRequestBody.ClientId"/>: The client ID for authentication.
    /// - <see cref="SafAccessTokenRequestBody.ClientSecret"/>: The client secret for authentication.
    /// - <see cref="SafAccessTokenRequestBody.Scope"/>: The scope of the requested token.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a
    /// <see cref="SafBearerTokenResponse"/> object with the bearer token and related metadata.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue with the HTTP request, such as a network error or invalid response.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if there is an issue deserializing the response from the SAF API.
    /// </exception>
    Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request);

    /// <summary>
    /// Asynchronously enrolls a technical user in the SAF system.
    /// </summary>
    /// <param name="request">
    /// The request object containing the technical user's enrollment information.
    /// Includes:
    /// - Bearer token for authentication
    /// - Technical user's credentials and details
    /// - Enrollment parameters and configurations
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a
    /// <see cref="SafTechUserEnrolmentResponse"/> object with the enrollment status and user details.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue with the HTTP request.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the bearer token is invalid or expired.
    /// </exception>
    Task<SafTechUserEnrolmentResponse> EnrolTechUserAsync(SafTechUserEnrolmentRequest request);

    /// <summary>
    /// Asynchronously retrieves the OpenID configuration from the specified URL.
    /// </summary>
    /// <param name="openIdUrl">
    /// The URL of the OpenID configuration endpoint.
    /// This URL should point to the well-known OpenID configuration document.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a
    /// <see cref="SafOpenIdConfigurationResponse"/> object with the OpenID configuration details.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue retrieving the configuration.
    /// </exception>
    /// <exception cref="UriFormatException">
    /// Thrown if the provided URL is not properly formatted.
    /// </exception>
    Task<SafOpenIdConfigurationResponse> GetOpenIdConfigurationAsync(Uri openIdUrl);
}
