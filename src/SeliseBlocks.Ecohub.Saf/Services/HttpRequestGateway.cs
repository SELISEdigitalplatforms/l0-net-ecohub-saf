using System;
using System.Net.Http.Headers;
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

    public async Task<TResponse> GetAsync<TResponse>(
        string endpoint,
        Dictionary<string, string>? headers = null,
        string? bearerToken = null
    ) where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
        AddHeaders(request, headers, bearerToken);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>()
               ?? throw new InvalidOperationException("Failed to deserialize response.");
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest body,
        Dictionary<string, string>? headers = null,
        string? bearerToken = null,
        string? contentType = "application/json"
    )
    where TRequest : class
    where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        return await PostAsync<TRequest, TResponse>(request, body, headers, bearerToken, contentType);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
        Uri url,
        TRequest body,
        Dictionary<string, string>? headers = null,
        string? bearerToken = null,
        string? contentType = "application/json"
    )
    where TRequest : class
    where TResponse : class
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        return await PostAsync<TRequest, TResponse>(request, body, headers, bearerToken, contentType);

    }
    private async Task<TResponse> PostAsync<TRequest, TResponse>(
        HttpRequestMessage request,
        TRequest body,
        Dictionary<string, string>? headers = null,
        string? bearerToken = null,
        string? contentType = "application/json"
    )
    where TRequest : class
    where TResponse : class
    {
        AddHeaders(request, headers, bearerToken);
        // Handle different content types
        request.Content = contentType switch
        {
            "application/json" => JsonContent.Create(body),
            "application/x-www-form-urlencoded" => new FormUrlEncodedContent(
                body as Dictionary<string, string>
                ?? throw new ArgumentException("Body must be Dictionary<string, string> for form-urlencoded")
            ),
            _ => new StringContent(
                body?.ToString() ?? string.Empty,
                Encoding.UTF8,
                contentType!
            )
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>()
               ?? throw new InvalidOperationException("Failed to deserialize response.");
    }

    // Helper to add headers/bearer token
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

}
