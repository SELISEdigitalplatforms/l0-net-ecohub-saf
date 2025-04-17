using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SeliseBlocks.Ecohub.Saf;

internal class HttpRequestGateway : IHttpRequestGateway
{
    private readonly HttpClient _httpClient;
    public HttpRequestGateway(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, string bearerToken, TRequest request)
    where TRequest : class
    where TResponse : class
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        try
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, request);
            response.EnsureSuccessStatusCode();
            var tokenResponse = await response.Content.ReadFromJsonAsync<TResponse>();
            return tokenResponse ?? throw new Exception("Failed to deserialize bearer token response.");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error on http request: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Error deserializing response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
        }
    }

    public async Task<TResponse> GetAsync<TResponse>(string endpoint, string bearerToken)
    where TResponse : class
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        try
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var tokenResponse = await response.Content.ReadFromJsonAsync<TResponse>();
            return tokenResponse ?? throw new Exception("Failed to deserialize bearer token response.");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error on http request: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Error deserializing response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
        }
    }

    public async Task<TResponse> PostFormUrlEncodedAsync<TResponse>(string endpoint, Dictionary<string, string> request)
    where TResponse : class
    {
        var content = new FormUrlEncodedContent(request);
        try
        {
            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var tokenResponse = await response.Content.ReadFromJsonAsync<TResponse>();
            return tokenResponse ?? throw new Exception("Failed to deserialize bearer token response.");
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error on http request: {ex.Message}", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception($"Error deserializing response: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
        }
    }
}
