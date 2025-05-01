using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class SafApiService : ISafApiService
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    public SafApiService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    public async Task<IEnumerable<SafReceiversResponse>> GetReceiversAsync(SafReceiversRequest request)
    {
        request.Validate();
        var response = await _httpRequestGateway.PostAsync<SafReceiversRequestPayload, IEnumerable<SafReceiversResponse>>(
            endpoint: SafDriverConstant.GetReceiversEndpoint,
            request: request.Payload,
            headers: null,
            bearerToken: request.BearerToken);

        return response;
    }

    /// <summary>
    /// Asynchronously retrieves the public key of a member from the SAF API.
    /// This method sends a request to the SAF API to get the public key of a member based on the provided IDP number.
    /// The request should include the necessary authentication details and the IDP number of the member whose public key is being requested.
    /// </summary>
    /// <param name="bearerToken">
    /// The bearer token obtained from the SAF API after successful authentication.
    /// This token is used to authorize the request to retrieve the member's public key.
    /// 
    /// </param>
    /// <param name="idpNumber"></param>
    /// <returns></returns>
    public async Task<SafMemberPublicKeyResponse> GetMemberPublicKey(string bearerToken, string idpNumber)
    {
        if (string.IsNullOrEmpty(bearerToken))
        {
            throw new ArgumentException("bearerToken cannot be null or empty.", nameof(bearerToken));
        }
        if (string.IsNullOrEmpty(idpNumber))
        {
            throw new ArgumentException("idpNumber cannot be null or empty.", nameof(idpNumber));
        }
        var endpoint = SafDriverConstant.GetMemberPublicKeyEndpoint.Replace("{idpNumber}", idpNumber);
        var response = await _httpRequestGateway.GetAsync<SafMemberPublicKeyResponse>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        return response;
    }

}
