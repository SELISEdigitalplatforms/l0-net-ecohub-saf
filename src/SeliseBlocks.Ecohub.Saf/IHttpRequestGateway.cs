using System;

namespace SeliseBlocks.Ecohub.Saf;

public interface IHttpRequestGateway
{
    Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint
    , TRequest request
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null
    , string? contentType = "application/json")
    where TRequest : class
    where TResponse : class;
    Task<TResponse> PostAsync<TRequest, TResponse>(Uri url
    , TRequest request
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null
    , string? contentType = "application/json")
    where TRequest : class
    where TResponse : class;
    Task<TResponse> GetAsync<TResponse>(string endpoint
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null)
        where TResponse : class;
}
