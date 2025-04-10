using System;

namespace SeliseBlocks.Ecohub.Saf;

internal interface IHttpRequestGateway
{
    Task<TResponse> PostAsync<TRequest, TResponse>(string requestUrl, string bearerToken, TRequest request)
    where TRequest : class
    where TResponse : class;
    Task<TResponse> PostFormUrlEncodedAsync<TResponse>(string requestUrl, Dictionary<string, string> request)
    where TResponse : class;
}
