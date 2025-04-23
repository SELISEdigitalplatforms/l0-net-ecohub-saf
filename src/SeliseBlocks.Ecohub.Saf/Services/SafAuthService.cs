using System;
using System.Net.Http.Json;
using System.Text.Json;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// This class is responsible for handling authentication with the SAF API.
/// It uses the HttpClient to send requests to the SAF API and retrieve bearer tokens.
/// </summary>
public class SafAuthService : ISafAuthService
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    public SafAuthService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    /// <summary>
    /// This method retrieves a bearer token from the SAF API.
    /// It sends a POST request to the specified URL with the required parameters in the body.
    /// </summary>
    /// <param name="request">
    /// </param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request)
    {
        var formData = new Dictionary<string, string>
                        {
                            { "grant_type", request.Body.GrantType },
                            { "client_id", request.Body.ClientId },
                            { "client_secret", request.Body.ClientSecret },
                            { "scope", request.Body.Scope }
                        };
        var response = await _httpRequestGateway.PostAsync<Dictionary<string, string>, SafBearerTokenResponse>(
            request.RequestUrl, formData, null, string.Empty, "application/x-www-form-urlencoded");

        return response;
    }
}
