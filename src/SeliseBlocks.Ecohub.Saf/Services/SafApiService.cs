using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class SafApiService : ISafApiService
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    public SafApiService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    public async Task<SafReceiversResponse> GetReceiversAsync(SafReceiversRequest request)
    {
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<IEnumerable<SafReceiver>, SafReceiversResponse>();
        }
        var response = await _httpRequestGateway.PostAsync<SafReceiversRequestPayload, IEnumerable<SafReceiver>>(
            endpoint: SafDriverConstant.GetReceiversEndpoint,
            requestBody: request.Payload,
            headers: null,
            bearerToken: request.BearerToken);

        return response.MapToDerivedResponse<IEnumerable<SafReceiver>, SafReceiversResponse>();
    }

    public async Task<SafMemberPublicKeyResponse> GetMemberPublicKey(string bearerToken, string idpNumber)
    {
        var response = new SafMemberPublicKeyResponse();

        if (string.IsNullOrEmpty(bearerToken))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "bearerToken cannot be null or empty."
            };
            return response;
        }
        if (string.IsNullOrEmpty(idpNumber))
        {
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "idpNumber cannot be null or empty."
            };
            return response;
        }

        var endpoint = SafDriverConstant.GetMemberPublicKeyEndpoint.Replace("{idpNumber}", idpNumber);
        var safResponse = await _httpRequestGateway.GetAsync<SafMemberPublicKey>(
            endpoint: endpoint,
            headers: null,
            bearerToken: bearerToken);

        response = safResponse.MapToDerivedResponse<SafMemberPublicKey, SafMemberPublicKeyResponse>();

        return response;
    }

}
