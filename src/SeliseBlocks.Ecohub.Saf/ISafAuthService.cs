using System;
using SeliseBlocks.Ecohub.Saf;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// This interface is responsible for handling authentication with the SAF API.
/// </summary>
public interface ISafAuthService
{
    /// <summary>
    /// This method is responsible for obtaining a bearer token from the SAF API.
    /// It takes a SafBearerTokenRequest object as a parameter, which contains the request URL and the body of the request.
    /// The body of the request should be of type SafAccessTokenRequestBody, which contains properties such as grant type, client ID, client secret, and scope.
    /// The method returns a Task of type SafBearerTokenResponse, which contains the bearer token and other related information.
    /// The bearer token is used to authorize requests to the SAF API.
    /// </summary>
    /// <param name="request">
    /// The request object containing the URL and body for obtaining the bearer token.
    /// The body of the request should be of type SafAccessTokenRequestBody, which contains properties such as grant type, client ID, client secret, and scope.
    /// </param>
    /// <returns>
    /// A Task of type SafBearerTokenResponse, which contains the bearer token and other related information.
    /// The bearer token is used to authorize requests to the SAF API.
    /// The method may throw exceptions if there are issues with the request or response, such as HttpRequestException or JsonException.
    /// </returns>
    Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request);
}
