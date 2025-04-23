using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class SafAuthServiceTests
{
    private readonly Mock<IHttpRequestGateway> _httpRequestGatewayMock;
    private readonly SafAuthService _safAuthService;

    public SafAuthServiceTests()
    {
        _httpRequestGatewayMock = new Mock<IHttpRequestGateway>();
        _safAuthService = new SafAuthService(_httpRequestGatewayMock.Object);
    }

    [Fact]
    public async Task GetBearerToken_ShouldReturnToken_WhenRequestIsSuccessful()
    {
        // Arrange
        var request = new SafBearerTokenRequest
        {
            RequestUrl = "https://example.com/token",
            Body = new SafAccessTokenRequestBody
            {
                GrantType = "client_credentials",
                ClientId = "test-client-id",
                ClientSecret = "test-client-secret",
                Scope = "test-scope"
            }
        };

        var expectedResponse = new SafBearerTokenResponse
        {
            AccessToken = "test-access-token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ExtExpiresIn = 7200
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<Dictionary<string, string>, SafBearerTokenResponse>(
                request.RequestUrl,
                It.Is<Dictionary<string, string>>(formData =>
                    formData["grant_type"] == request.Body.GrantType &&
                    formData["client_id"] == request.Body.ClientId &&
                    formData["client_secret"] == request.Body.ClientSecret &&
                    formData["scope"] == request.Body.Scope),
                null,
                string.Empty,
                "application/x-www-form-urlencoded"))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safAuthService.GetBearerToken(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-access-token", result.AccessToken);
        Assert.Equal("Bearer", result.TokenType);
        Assert.Equal(3600, result.ExpiresIn);
        Assert.Equal(7200, result.ExtExpiresIn);
    }

    [Fact]
    public async Task GetBearerToken_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var request = new SafBearerTokenRequest
        {
            RequestUrl = "https://example.com/token",
            Body = new SafAccessTokenRequestBody
            {
                GrantType = "client_credentials",
                ClientId = "test-client-id",
                ClientSecret = "test-client-secret",
                Scope = "test-scope"
            }
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<Dictionary<string, string>, SafBearerTokenResponse>(
                request.RequestUrl,
                It.IsAny<Dictionary<string, string>>(),
                null,
                string.Empty,
                "application/x-www-form-urlencoded"))
            .ThrowsAsync(new Exception("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _safAuthService.GetBearerToken(request));
    }
}