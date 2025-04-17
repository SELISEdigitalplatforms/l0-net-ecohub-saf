using System;

namespace SeliseBlocks.Ecohub.Saf;

internal interface IHttpRequestGateway
{
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, string bearerToken, TRequest request)
    where TRequest : class
    where TResponse : class;
    Task<TResponse> GetAsync<TResponse>(string endpoint, string bearerToken)
        where TResponse : class;

    Task<TResponse> PostFormUrlEncodedAsync<TResponse>(string endpoint, Dictionary<string, string> request)
    where TResponse : class;
}
