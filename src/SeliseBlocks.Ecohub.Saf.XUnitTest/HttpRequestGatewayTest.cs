using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;
using SeliseBlocks.Ecohub.Saf.Services;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class HttpRequestGatewayTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly HttpRequestGateway _httpRequestGateway;

    public HttpRequestGatewayTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _httpRequestGateway = new HttpRequestGateway(_httpClient);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDeserializedResponse_WhenRequestIsSuccessful()
    {
        // Arrange
        var endpoint = "https://example.com/api/resource";
        var expectedResponse = new { Message = "Success" };
        var jsonResponse = JsonSerializer.Serialize(expectedResponse);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == endpoint),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _httpRequestGateway.GetAsync<object>(endpoint);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(expectedResponse.Message, ((JsonElement)result.Data).GetProperty("Message").GetString());
    }

    [Fact]
    public async Task PostAsync_ShouldReturnDeserializedResponse_WhenRequestIsSuccessful()
    {
        // Arrange
        var endpoint = "https://example.com/api/resource";
        var requestBody = new { Name = "Test" };
        var expectedResponse = new { Id = 1, Name = "Test" };
        var jsonResponse = JsonSerializer.Serialize(expectedResponse);

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString() == endpoint),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _httpRequestGateway.PostAsync<object, object>(endpoint, requestBody);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(expectedResponse.Id, ((JsonElement)result.Data).GetProperty("Id").GetInt32());
        Assert.Equal(expectedResponse.Name, ((JsonElement)result.Data).GetProperty("Name").GetString());
    }

    [Fact]
    public async Task PostAsync_ShouldReturnError_WhenHttpRequestFails()
    {
        // Arrange
        var endpoint = "https://example.com/api/resource";
        var requestBody = new { Name = "Test" };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await _httpRequestGateway.PostAsync<object, object>(endpoint, requestBody);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("HTTP request failed", result.Error.ErrorCode);
        Assert.Equal("Request failed", result.Error.ErrorMessage);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnError_WhenHttpRequestFails()
    {
        // Arrange
        var endpoint = "https://example.com/api/resource";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act
        var result = await _httpRequestGateway.GetAsync<object>(endpoint);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("HTTP request failed", result.Error.ErrorCode);
        Assert.Equal("Request failed", result.Error.ErrorMessage);
    }

    [Fact]
    public async Task PostAsync_ShouldReturnError_WhenDeserializationFails()
    {
        // Arrange
        var endpoint = "https://example.com/api/resource";
        var requestBody = new { Name = "Test" };
        var invalidJson = "{ invalid json }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(invalidJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _httpRequestGateway.PostAsync<object, object>(endpoint, requestBody);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Deserialization error", result.Error.ErrorCode);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnError_WhenDeserializationFails()
    {
        // Arrange
        var endpoint = "https://example.com/api/resource";
        var invalidJson = "{ invalid json }";

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(invalidJson, Encoding.UTF8, "application/json")
            });

        // Act
        var result = await _httpRequestGateway.GetAsync<object>(endpoint);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Deserialization error", result.Error.ErrorCode);
    }
}