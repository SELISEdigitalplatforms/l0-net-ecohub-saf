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
}
