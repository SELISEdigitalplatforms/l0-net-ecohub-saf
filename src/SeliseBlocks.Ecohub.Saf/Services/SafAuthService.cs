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
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<SafBearerToken, SafBearerTokenResponse>();
        }
        var formData = new Dictionary<string, string>
                        {
                            { "grant_type", request.Body.GrantType },
                            { "client_id", request.Body.ClientId },
                            { "client_secret", request.Body.ClientSecret },
                            { "scope", request.Body.Scope }
                        };
        var response = await _httpRequestGateway.PostAsync<Dictionary<string, string>, SafBearerToken>(
            endpoint: request.RequestUrl,
            requestBody: formData,
            headers: null,
            bearerToken: string.Empty,
            contentType: "application/x-www-form-urlencoded");

        return response.MapToDerivedResponse<SafBearerToken, SafBearerTokenResponse>();
    }

    /// <inheritdoc/>
    public async Task<SafTechUserEnrolmentResponse> EnrolTechUserAsync(SafTechUserEnrolmentRequest request)
    {
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<SafTechUserEnrolment, SafTechUserEnrolmentResponse>();
        }
        var response = await _httpRequestGateway.PostAsync<SafTechUserEnrolmentRequest, SafTechUserEnrolment>(
            endpoint: SafDriverConstant.TechUserEnrolmentEndpoint,
            requestBody: request);

        return response.MapToDerivedResponse<SafTechUserEnrolment, SafTechUserEnrolmentResponse>();
    }

    /// <inheritdoc/>
    public async Task<SafOpenIdConfigurationResponse> GetOpenIdConfigurationAsync(Uri openIdUrl)
    {
        if (openIdUrl is null)
        {
            return new SafOpenIdConfigurationResponse
            {
                Error = new SafError
                {
                    ErrorCode = "ValidationError",
                    ErrorMessage = "OpenID URL cannot be null."
                }
            };
        }
        var response = await _httpRequestGateway.GetAsync<SafOpenIdConfiguration>(
                        uri: openIdUrl);

        return response.MapToDerivedResponse<SafOpenIdConfiguration, SafOpenIdConfigurationResponse>();
    }
}
