using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class SafRestProxyEventHandler : ISafRestProxyEventHandler
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    public SafRestProxyEventHandler(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    public async Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request)
    {
        request.Validate();

        var header = new Dictionary<string, string>
        {
            { "schemaVersionId", request.SchemaVersionId },
            { "keySchemaVersionId", request.KeySchemaVersionId }
        };

        var payload = PrepareEventRequestPayload(request.EventPayload);
        var response = await _httpRequestGateway.PostAsync<SafOfferNlpiEncryptedEvent, SafSendOfferNlpiEventResponse>(
            endpoint: SafDriverConstant.SendOfferNlpiEventEndpoint,
            request: payload,
            headers: header,
            bearerToken: request.BearerToken);

        return response;
    }
    public async Task<IEnumerable<SafOfferNlpiEvent>> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request)
    {
        request.Validate();

        var endpoint = SafDriverConstant.ReceiveOfferNlpiEventEndpoint.Replace("{ecohubId}", request.EcohubId);
        var header = new Dictionary<string, string>
        {
            { "auto.offset.reset", request.AutoOffsetReset }
        };
        var eventResponses = await _httpRequestGateway.GetAsync<IEnumerable<SafReceiveOfferNlpiEventResponse>>(
            endpoint: endpoint,
            headers: header,
            bearerToken: request.BearerToken);

        var events = new List<SafOfferNlpiEvent>();
        foreach (var eventItem in eventResponses)
        {
            var eventResponse = PrepareEventResponsePayload(eventItem, request.PrivateKey);
            events.Add(eventResponse);
        }

        return events;
    }

    private SafOfferNlpiEncryptedEvent PrepareEventRequestPayload(SafOfferNlpiEvent eventPayload)
    {
        var payload = eventPayload.MapToSafOfferNlpiEncryptedEvent();
        payload.Data = SafEventDataResolver.CompressAndEncrypt(eventPayload.Data);
        return payload;
    }
    private SafOfferNlpiEvent PrepareEventResponsePayload(SafReceiveOfferNlpiEventResponse eventItem, string privateKey)
    {
        var eventResponse = eventItem.Value.MapToSafOfferNlpiEvent();

        var data = SafEventDataResolver.DecryptAndDecompress(eventItem.Value.Data, privateKey);
        eventResponse.Data = data;
        return eventResponse;
    }


}
