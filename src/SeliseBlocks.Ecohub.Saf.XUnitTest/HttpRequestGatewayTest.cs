using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;
using SeliseBlocks.Ecohub.Saf.Services;

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
        Assert.Equal(expectedResponse.Message, ((JsonElement)result).GetProperty("Message").GetString());
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
        Assert.Equal(expectedResponse.Id, ((JsonElement)result).GetProperty("Id").GetInt32());
        Assert.Equal(expectedResponse.Name, ((JsonElement)result).GetProperty("Name").GetString());
    }

    [Fact]
    public async Task PostAsync_ShouldThrowInvalidOperationException_WhenHttpRequestFails()
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

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _httpRequestGateway.PostAsync<object, object>(endpoint, requestBody));
    }

    [Fact]
    public async Task GetAsync_ShouldThrowInvalidOperationException_WhenHttpRequestFails()
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

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _httpRequestGateway.GetAsync<object>(endpoint));
    }
}