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

        var response = await _httpRequestGateway.PostAsync<SafReceiversRequestPayload, SafReceiversResponse>(
            SafDriverConstant.GetReceiversEndpoint,
            request.Payload,
            null,
            request.BearerToken);

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
        var endpoint = SafDriverConstant.GetMemberPublicKeyEndpoint.Replace("{idpNumber}", idpNumber);
        var response = await _httpRequestGateway.GetAsync<SafMemberPublicKeyResponse>(
            endpoint,
            null,
            bearerToken);

        return response;
    }

    public async Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request)
    {
        var header = new Dictionary<string, string>
        {
            { "schemaVersionId", request.SchemaVersionId },
            { "keySchemaVersionId", request.KeySchemaVersionId }
        };
        var response = await _httpRequestGateway.PostAsync<SafOfferNlpiEvent, SafSendOfferNlpiEventResponse>(
            SafDriverConstant.SendOfferNlpiEventEndpoint,
            request.EventPayload,
            header,
            request.BearerToken);

        return response;
    }
    public async Task<SafReceiveOfferNlpiEventResponse> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request)
    {
        var endpoint = SafDriverConstant.ReceiveOfferNlpiEventEndpoint.Replace("{ecohubId}", request.EcohubId);
        var header = new Dictionary<string, string>
        {
            { "auto.offset.reset", request.AutoOffsetReset }
        };
        var response = await _httpRequestGateway.GetAsync<SafReceiveOfferNlpiEventResponse>(
            endpoint,
            header,
            request.BearerToken);

        return response;
    }
}
