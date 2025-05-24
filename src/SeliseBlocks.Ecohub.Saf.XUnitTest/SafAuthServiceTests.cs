using Moq;
using SeliseBlocks.Ecohub.Saf.Helpers;
using SeliseBlocks.Ecohub.Saf.Services;

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

        var expectedResponse = new SafBearerToken
        {
            AccessToken = "test-access-token",
            TokenType = "Bearer",
            ExpiresIn = 3600,
            ExtExpiresIn = 7200
        };

        var safBaseResponse = new SafBaseResponse<SafBearerToken>
        {
            IsSuccess = true,
            Data = expectedResponse
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<Dictionary<string, string>, SafBearerToken>(
                request.RequestUrl,
                It.Is<Dictionary<string, string>>(formData =>
                    formData["grant_type"] == request.Body.GrantType &&
                    formData["client_id"] == request.Body.ClientId &&
                    formData["client_secret"] == request.Body.ClientSecret &&
                    formData["scope"] == request.Body.Scope),
                null,
                string.Empty,
                "application/x-www-form-urlencoded"))
            .ReturnsAsync(safBaseResponse);
        // Act
        var result = await _safAuthService.GetBearerToken(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-access-token", result.Data?.AccessToken);
        Assert.Equal("Bearer", result.Data?.TokenType);
        Assert.Equal(3600, result.Data?.ExpiresIn);
        Assert.Equal(7200, result.Data?.ExtExpiresIn);
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
            .Setup(x => x.PostAsync<Dictionary<string, string>, SafBearerToken>(
                request.RequestUrl,
                It.IsAny<Dictionary<string, string>>(),
                null,
                string.Empty,
                "application/x-www-form-urlencoded"))
            .ThrowsAsync(new Exception("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _safAuthService.GetBearerToken(request));
    }

    [Fact]
    public async Task EnrolTechUserAsync_ShouldReturnResponse_WhenRequestIsValid()
    {
        // Arrange
        var request = new SafTechUserEnrolmentRequest
        {
            Iak = "test-iak",
            IdpUserId = "test-user-id",
            LicenceKey = "test-licence",
            Password = "test-password",
            RequestId = Guid.NewGuid().ToString(),
            RequestTime = DateTime.UtcNow.ToString("o"),
            UserAgent = new SafUserAgent
            {
                Name = "test-agent",
                Version = "1.0"
            }
        };

        var expectedResponse = new SafTechUserEnrolment
        {
            TechUserCert = "test-cert",
            OAuth2 = new SafTechUserEnrolmentOauth2
            {
                ClientId = "test-client-id",
                ClientSecret = "test-client-secret",
                OpenIdConfigurationEndpoint = "https://test.com/openid"
            }
        };

        var safBaseResponse = new SafBaseResponse<SafTechUserEnrolment>
        {
            IsSuccess = true,
            Data = expectedResponse
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafTechUserEnrolmentRequest, SafTechUserEnrolment>(
                It.Is<string>(url => url == SafDriverConstant.TechUserEnrolmentEndpoint),
                It.Is<SafTechUserEnrolmentRequest>(r => r.Iak == request.Iak),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(safBaseResponse);

        // Act
        var result = await _safAuthService.EnrolTechUserAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.TechUserCert, result.Data?.TechUserCert);
        Assert.Equal(expectedResponse.OAuth2.ClientId, result.Data?.OAuth2.ClientId);
        Assert.Equal(expectedResponse.OAuth2.ClientSecret, result.Data?.OAuth2.ClientSecret);
    }

    [Fact]
    public async Task GetOpenIdConfigurationAsync_ShouldReturnResponse_WhenUrlIsValid()
    {
        // Arrange
        var openIdUrl = new Uri("https://test.com/openid");
        var expectedResponse = new SafOpenIdConfiguration
        {
            TokenEndpoint = "https://test.com/token",
            Issuer = "test-issuer",
            AuthorizationEndpoint = "https://test.com/auth",
            UserinfoEndpoint = "https://test.com/userinfo",
            DeviceAuthorizationEndpoint = "https://test.com/device",
            EndSessionEndpoint = "https://test.com/logout",
            TokenEndpointAuthMethodsSupported = new[] { "client_secret_post" },
            ResponseModesSupported = new[] { "query" },
            SubjectTypesSupported = new[] { "public" },
            ResponseTypesSupported = new[] { "code" },
            ScopesSupported = new[] { "openid" }
        };

        var safBaseResponseOpenId = new SafBaseResponse<SafOpenIdConfiguration>
        {
            IsSuccess = true,
            Data = expectedResponse
        };

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<SafOpenIdConfiguration>(
                It.Is<Uri>(uri => uri == openIdUrl),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>()))
            .ReturnsAsync(safBaseResponseOpenId);
        // Act
        var result = await _safAuthService.GetOpenIdConfigurationAsync(openIdUrl);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.TokenEndpoint, result.Data?.TokenEndpoint);
        Assert.Equal(expectedResponse.Issuer, result.Data?.Issuer);
        Assert.Equal(expectedResponse.AuthorizationEndpoint, result.Data?.AuthorizationEndpoint);
        Assert.Equal(expectedResponse.UserinfoEndpoint, result.Data?.UserinfoEndpoint);
    }

    [Fact]
    public async Task GetOpenIdConfigurationAsync_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var openIdUrl = new Uri("https://test.com/openid");
        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<SafOpenIdConfiguration>(
                It.IsAny<Uri>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _safAuthService.GetOpenIdConfigurationAsync(openIdUrl));
    }
}