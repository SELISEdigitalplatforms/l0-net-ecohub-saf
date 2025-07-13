using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace SeliseBlocks.Ecohub.Saf.Services;

public class HttpRequestGateway : IHttpRequestGateway
{
    private readonly HttpClient _httpClient;
    public HttpRequestGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    #region Get Methods
    public async Task<SafBaseResponse<TResponse>> GetAsync<TResponse>(
        string endpoint,
        Dictionary<string, string>? headers = null,
        string? bearerToken = null)
        where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        return await SendAsync<Null, TResponse>(request, null, headers, bearerToken);
    }

    public async Task<SafBaseResponse<TResponse>> GetAsync<TResponse>(
        Uri requestUri,
        Dictionary<string, string>? headers = null,
        string? bearerToken = null)
        where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        return await SendAsync<Null, TResponse>(request, null, headers, bearerToken);
    }

    #endregion Get Methods

    #region Post Methods

    public async Task<SafBaseResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint
    , TRequest? requestBody
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null
    , string? contentType = "application/json")
    where TRequest : class
    where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        return await SendAsync<TRequest, TResponse>(request, requestBody, headers, bearerToken, contentType);
    }

    public async Task<SafBaseResponse<TResponse>> PostAsync<TRequest, TResponse>(Uri url
    , TRequest? requestBody
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null
    , string? contentType = "application/json")
    where TRequest : class
    where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        return await SendAsync<TRequest, TResponse>(request, requestBody, headers, bearerToken, contentType);

    }

    #endregion Post Methods

    #region Delete Methods
    public async Task<SafBaseResponse<TResponse>> DeleteAsync<TRequest, TResponse>(string endpoint
    , TRequest? requestBody
    , Dictionary<string, string>? headers = null
    , string? bearerToken = null
    , string? contentType = "application/json")
    where TRequest : class
    where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
        return await SendAsync<TRequest, TResponse>(request, requestBody, headers, bearerToken, contentType);
    }
    #endregion

    #region Private Methods

    private async Task<SafBaseResponse<TResponse>> SendAsync<TRequest, TResponse>(
        HttpRequestMessage request,
        TRequest? body,
        Dictionary<string, string>? headers = null,
        string? bearerToken = null,
        string? contentType = "application/json"
    )
    where TRequest : class
    where TResponse : class
    {
        var response = new SafBaseResponse<TResponse>();
        try
        {
            AddHeaders(request, headers, bearerToken);
            // Handle different content types
            if (contentType == "application/x-www-form-urlencoded")
            {
                request.Content = new FormUrlEncodedContent(
                    body as Dictionary<string, string>
                    ?? throw new ArgumentException("Body must be Dictionary<string, string> for form-urlencoded"));
                request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded");
            }
            else if (body is not null && contentType == "application/json")
            {
                request.Content = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(
                        body,
                        new Newtonsoft.Json.JsonSerializerSettings
                        {
                            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
                        }
                    ),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            var httpResponse = await _httpClient.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK
            || httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest
            || httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound
            || httpResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError
            || httpResponse.StatusCode == System.Net.HttpStatusCode.Conflict
            || httpResponse.StatusCode == System.Net.HttpStatusCode.UnprocessableContent)
            {
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    response.IsSuccess = true;
                    response.Data = string.IsNullOrWhiteSpace(responseContent)
                    ? null : await httpResponse.Content.ReadFromJsonAsync<TResponse>();
                }
                else
                {
                    response.IsSuccess = false;
                    response.Error = await httpResponse.Content.ReadFromJsonAsync<SafError>();
                }
            }
        }
        catch (HttpRequestException ex)
        {
            response.IsSuccess = false;
            response.Error = new SafError
            {
                ErrorMessage = ex.Message,
                ErrorCode = "HTTP request failed"
            };
        }
        catch (JsonException ex)
        {
            response.IsSuccess = false;
            response.Error = new SafError
            {
                ErrorMessage = ex.Message,
                ErrorCode = "Deserialization error"
            };
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Error = new SafError
            {
                ErrorMessage = ex.Message,
                ErrorCode = "Unexpected error"
            };
        }
        return response;
    }
    private void AddHeaders(
        HttpRequestMessage request,
        Dictionary<string, string>? headers,
        string? bearerToken
    )
    {
        if (bearerToken != null)
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }
    }
    #endregion Private Methods

}
