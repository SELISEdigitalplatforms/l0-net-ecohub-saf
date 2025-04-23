using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SeliseBlocks.Ecohub.Saf;

public class HttpRequestGateway : IHttpRequestGateway
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
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            AddHeaders(request, headers, bearerToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseContent) || responseContent == "[]" || responseContent == "{}")
            {
                return null!;
            }

            return JsonSerializer.Deserialize<TResponse>(responseContent)
                   ?? throw new InvalidOperationException("Failed to deserialize response.");

        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("HTTP request failed.", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to deserialize response.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred.", ex);
        }
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
            else
            {
                request.Content = new StringContent(
                    JsonSerializer.Serialize(body),
                    Encoding.UTF8,
                    "application/json"
                );
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseContent) || responseContent == "[]" || responseContent == "{}")
            {
                return null!;
            }

            return await response.Content.ReadFromJsonAsync<TResponse>()
                   ?? throw new InvalidOperationException("Failed to deserialize response.");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("HTTP request failed.", ex);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to deserialize response.", ex);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An unexpected error occurred.", ex);
        }
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
