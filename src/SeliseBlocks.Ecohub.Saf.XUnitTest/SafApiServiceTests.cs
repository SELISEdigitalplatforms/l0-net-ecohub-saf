using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class SafApiServiceTests
{
    private readonly Mock<IHttpRequestGateway> _httpRequestGatewayMock;
    private readonly SafApiService _safApiService;

    public SafApiServiceTests()
    {
        _httpRequestGatewayMock = new Mock<IHttpRequestGateway>();
        _safApiService = new SafApiService(_httpRequestGatewayMock.Object);
    }

    [Fact]
    public async Task GetReceiversAsync_ShouldReturnReceivers_WhenRequestIsSuccessful()
    {
        // Arrange
        var request = new SafReceiversRequest
        {
            BearerToken = "test-bearer-token",
            Payload = new SafReceiversRequestPayload
            {
                LicenceKey = "test-licence-key",
                Password = "test-password",
                RequestId = Guid.NewGuid().ToString(),
                RequestTime = DateTime.UtcNow.ToString("o"),
                UserAgent = new SafUserAgent
                {
                    Name = "Chrome",
                    Version = "Desktop"
                }
            }
        };

        var expectedResponse = new List<SafReceiversResponse>
        {
            new SafReceiversResponse
            {
                Idp = new List<string> { "IDP1", "IDP2" },
                CompanyName = "Test Company",
                MemberType = "Test Member",
                SafSupportedStandards = new List<SafSupportedStandard>
                {
                    new SafSupportedStandard
                    {
                        ProcessName = "Test Process",
                        ProcessVersion = "1.0"
                    }
                }
            }
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafReceiversRequestPayload, IEnumerable<SafReceiversResponse>>(
                "ReceiversEndpoint",
                request.Payload,
                null,
                request.BearerToken,
                "application/json"))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safApiService.GetReceiversAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Company", result.First().CompanyName);
    }


    [Fact]
    public async Task GetMemberPublicKey_ShouldReturnPublicKey_WhenRequestIsSuccessful()
    {
        // Arrange
        var bearerToken = "test-bearer-token";
        var idpNumber = "12345";
        var expectedResponse = new SafMemberPublicKeyResponse
        {
            KeyId = "key-id",
            MembershipId = "membership-id",
            Version = "1.0",
            Key = "public-key",
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            ActivatedAt = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddYears(1),
            EcoHubStatus = "Active"
        };

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<SafMemberPublicKeyResponse>(
                It.Is<string>(url => url.Contains(idpNumber)),
                null,
                bearerToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safApiService.GetMemberPublicKey(bearerToken, idpNumber);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("key-id", result.KeyId);
        Assert.Equal("public-key", result.Key);
    }

    [Fact]
    public async Task GetMemberPublicKey_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var bearerToken = "test-bearer-token";
        var idpNumber = "12345";

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<SafMemberPublicKeyResponse>(
                It.Is<string>(url => url.Contains(idpNumber)),
                null,
                bearerToken))
            .ThrowsAsync(new Exception("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _safApiService.GetMemberPublicKey(bearerToken, idpNumber));
    }
}