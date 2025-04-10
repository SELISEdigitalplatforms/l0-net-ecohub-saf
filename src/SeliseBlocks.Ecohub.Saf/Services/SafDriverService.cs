using System;
using SeliseBlocks.Ecohub.Saf.Models.RequestModels;

namespace SeliseBlocks.Ecohub.Saf;

internal class SafDriverService : ISafDriverService
{
    private readonly IHttpRequestGateway _httpRequestGateway;
    public SafDriverService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }
    public async Task<SafReceiversResponse> GetReceiversAsync(SafReceiversRequest request)
    {
        var response = await _httpRequestGateway.PostAsync<SafReceiversRequestBody, SafReceiversResponse>(
            request.RequestUrl,
            request.BearerToken,
            request.Body);

        return response;
    }
}
