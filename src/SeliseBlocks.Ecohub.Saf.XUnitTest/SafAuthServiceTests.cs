using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SeliseBlocks.Ecohub.Saf.Services;
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

        var expectedResponse = new SafTechUserEnrolmentResponse
        {
            TechUserCert = "test-cert",
            OAuth2 = new SafTechUserEnrolmentOauth2Response
            {
                ClientId = "test-client-id",
                ClientSecret = "test-client-secret",
                OpenIdConfigurationEndpoint = "https://test.com/openid"
            }
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafTechUserEnrolmentRequest, SafTechUserEnrolmentResponse>(
                It.Is<string>(url => url == SafDriverConstant.TechUserEnrolmentEndpoint),
                It.Is<SafTechUserEnrolmentRequest>(r => r.Iak == request.Iak),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safAuthService.EnrolTechUserAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.TechUserCert, result.TechUserCert);
        Assert.Equal(expectedResponse.OAuth2.ClientId, result.OAuth2.ClientId);
        Assert.Equal(expectedResponse.OAuth2.ClientSecret, result.OAuth2.ClientSecret);
    }

    [Fact]
    public async Task GetOpenIdConfigurationAsync_ShouldReturnResponse_WhenUrlIsValid()
    {
        // Arrange
        var openIdUrl = new Uri("https://test.com/openid");
        var expectedResponse = new SafOpenIdConfigurationResponse
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

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<SafOpenIdConfigurationResponse>(
                It.Is<Uri>(uri => uri == openIdUrl),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safAuthService.GetOpenIdConfigurationAsync(openIdUrl);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.TokenEndpoint, result.TokenEndpoint);
        Assert.Equal(expectedResponse.Issuer, result.Issuer);
        Assert.Equal(expectedResponse.AuthorizationEndpoint, result.AuthorizationEndpoint);
        Assert.Equal(expectedResponse.UserinfoEndpoint, result.UserinfoEndpoint);
    }

    [Fact]
    public async Task GetOpenIdConfigurationAsync_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var openIdUrl = new Uri("https://test.com/openid");
        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<SafOpenIdConfigurationResponse>(
                It.IsAny<Uri>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>()))
            .ThrowsAsync(new InvalidOperationException("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _safAuthService.GetOpenIdConfigurationAsync(openIdUrl));
    }
}