using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.Services;

/// <summary>
/// Implementation of the <see cref="ISafAuthService"/> interface.
/// Handles authentication operations with the SAF API, including token management,
/// technical user enrollment, and OpenID configuration retrieval.
/// </summary>
public class SafAuthService : ISafAuthService
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    /// <summary>
    /// Initializes a new instance of the <see cref="SafAuthService"/> class.
    /// </summary>
    /// <param name="httpRequestGateway">
    /// The HTTP request gateway used to make requests to the SAF API.
    /// This gateway handles the actual HTTP communication and response processing.
    /// </param>
    public SafAuthService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    /// <inheritdoc/>
    public async Task<SafBearerTokenResponse> GetBearerToken(SafBearerTokenRequest request)
    {
        request.Validate();
        var formData = new Dictionary<string, string>
                        {
                            { "grant_type", request.Body.GrantType },
                            { "client_id", request.Body.ClientId },
                            { "client_secret", request.Body.ClientSecret },
                            { "scope", request.Body.Scope }
                        };
        var response = await _httpRequestGateway.PostAsync<Dictionary<string, string>, SafBearerTokenResponse>(
            endpoint: request.RequestUrl,
            request: formData,
            headers: null,
            bearerToken: string.Empty,
            contentType: "application/x-www-form-urlencoded");

        return response;
    }

    /// <inheritdoc/>
    public async Task<SafTechUserEnrolmentResponse> EnrolTechUserAsync(SafTechUserEnrolmentRequest request)
    {
        request.Validate();
        var response = await _httpRequestGateway.PostAsync<SafTechUserEnrolmentRequest, SafTechUserEnrolmentResponse>(
            endpoint: SafDriverConstant.TechUserEnrolmentEndpoint,
            request: request);

        return response;
    }

    /// <inheritdoc/>
    public async Task<SafOpenIdConfigurationResponse> GetOpenIdConfigurationAsync(Uri openIdUrl)
    {
        if (openIdUrl == null)
        {
            throw new ArgumentNullException(nameof(openIdUrl), "OpenID URL cannot be null.");
        }
        var response = await _httpRequestGateway.GetAsync<SafOpenIdConfigurationResponse>(
                        uri: openIdUrl);

        return response;
    }
}
