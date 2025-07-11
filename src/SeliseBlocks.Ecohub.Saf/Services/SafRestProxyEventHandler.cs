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
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<SafSendOfferNlpiEvent, SafSendOfferNlpiEventResponse>();
        }

        var header = new Dictionary<string, string>
        {
            { "schemaVersionId", request.SchemaVersionId },
            { "keySchemaVersionId", request.KeySchemaVersionId }
        };

        var payload = PrepareEventRequestPayload(request.EventPayload);
        var response = await _httpRequestGateway.PostAsync<SafOfferNlpiEncryptedEvent, SafSendOfferNlpiEvent>(
            endpoint: SafDriverConstant.SendOfferNlpiEventEndpoint,
            requestBody: payload,
            headers: header,
            bearerToken: request.BearerToken);

        return response.MapToDerivedResponse<SafSendOfferNlpiEvent, SafSendOfferNlpiEventResponse>();
    }
    public async Task<SafReceiveOfferNlpiEventResponse> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request)
    {
        var validation = request.Validate();
        if (!validation.IsSuccess)
        {
            return validation.MapToResponse<IEnumerable<SafOfferNlpiEvent>, SafReceiveOfferNlpiEventResponse>();
        }

        var endpoint = SafDriverConstant.ReceiveOfferNlpiEventEndpoint.Replace("{ecohubId}", request.EcohubId);
        var header = new Dictionary<string, string>
        {
            { "auto.offset.reset", request.AutoOffsetReset }
        };
        var eventResponse = await _httpRequestGateway.GetAsync<IEnumerable<SafReceiveOfferNlpiEvent>>(
            endpoint: endpoint,
            headers: header,
            bearerToken: request.BearerToken);

        if (!eventResponse.IsSuccess)
            return new SafReceiveOfferNlpiEventResponse
            {
                IsSuccess = false,
                Error = eventResponse.Error
            };

        var events = new List<SafOfferNlpiEvent>();
        foreach (var eventItem in eventResponse.Data)
        {
            var eventResponsePayload = PrepareEventResponsePayload(eventItem, request.PrivateKey);
            events.Add(eventResponsePayload);
        }

        return new SafReceiveOfferNlpiEventResponse
        {
            IsSuccess = true,
            Data = events
        };
    }

    private SafOfferNlpiEncryptedEvent PrepareEventRequestPayload(SafOfferNlpiEvent eventPayload)
    {
        var payload = eventPayload.MapToSafOfferNlpiEncryptedEvent();
        payload.Data = SafEventDataResolver.CompressEncryptAndSignPayload(eventPayload.Data);
        return payload;
    }
    private SafOfferNlpiEvent PrepareEventResponsePayload(SafReceiveOfferNlpiEvent eventItem, string privateKey)
    {
        var eventResponse = eventItem.Value.MapToSafOfferNlpiEvent();

        var data = SafEventDataResolver.DecryptAndDecompress(eventItem.Value.Data, privateKey);
        eventResponse.Data = data;
        return eventResponse;
    }


}
