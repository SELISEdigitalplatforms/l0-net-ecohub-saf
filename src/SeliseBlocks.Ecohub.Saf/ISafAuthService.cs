using System;
using SeliseBlocks.Ecohub.Saf;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Interface for handling authentication with the SAF API.
/// Provides methods to obtain a bearer token required for authorizing requests to the SAF API.
/// </summary>
public interface ISafAuthService
{
    /// <summary>
    /// Asynchronously retrieves a bearer token from the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the URL and body for obtaining the bearer token.
    /// The body should include the following parameters:
    /// - <see cref="SafAccessTokenRequestBody.GrantType"/>: The grant type for the token request (e.g., "client_credentials").
    /// - <see cref="SafAccessTokenRequestBody.ClientId"/>: The client ID for authentication.
    /// - <see cref="SafAccessTokenRequestBody.ClientSecret"/>: The client secret for authentication.
    /// - <see cref="SafAccessTokenRequestBody.Scope"/>: The scope of the requested token.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a 
    /// <see cref="SafBearerTokenResponse"/> object, which includes the bearer token and related metadata.
    /// </returns>
    /// <exception cref="HttpRequestException">
    /// Thrown if there is an issue with the HTTP request, such as a network error or invalid response.
    /// </exception>
    /// <exception cref="JsonException">
    /// Thrown if there is an issue deserializing the response from the SAF API.
    /// </exception>
    Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request);
}
