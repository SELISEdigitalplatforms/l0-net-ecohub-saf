namespace SeliseBlocks.Ecohub.Saf;

public interface IHttpRequestGateway
{
    Task<SafBaseResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint
    , TRequest? requestBody
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null
    , string? contentType = "application/json")
    where TRequest : class
    where TResponse : class;
    Task<SafBaseResponse<TResponse>> PostAsync<TRequest, TResponse>(Uri url
    , TRequest? requestBody
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null
    , string? contentType = "application/json")
    where TRequest : class
    where TResponse : class;

    Task<SafBaseResponse<TResponse>> DeleteAsync<TRequest, TResponse>(string endpoint
    , TRequest? requestBody
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null
    , string? contentType = "application/json")
    where TRequest : class
    where TResponse : class;
    Task<SafBaseResponse<TResponse>> GetAsync<TResponse>(string endpoint
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null)
        where TResponse : class;
    Task<SafBaseResponse<TResponse>> GetAsync<TResponse>(Uri uri
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null)
        where TResponse : class;
}
