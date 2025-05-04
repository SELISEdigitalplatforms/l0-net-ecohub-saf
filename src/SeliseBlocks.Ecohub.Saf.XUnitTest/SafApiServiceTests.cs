using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SeliseBlocks.Ecohub.Saf.Helpers;
using SeliseBlocks.Ecohub.Saf.Services;
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

    [Fact]
    public async Task UploadMemberPublicKey_ShouldReturnResponse_WhenRequestIsSuccessful()
    {
        // Arrange  
        var request = new SafMemberPublicKeyUploadRequest
        {
            BearerToken = "test-bearer-token",
            Payload = new SafMemberPublicKeyUploadRequestPayload
            {
                Key = "public-key",
                Version = "1.0",
                ExpireInDays = "7"
            }
        };

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
            EcoHubStatus = "Created"
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafMemberPublicKeyUploadRequest, SafMemberPublicKeyResponse>(
                SafDriverConstant.UploadMemberPublicKeyEndpoint,
                request,
                null, // headers  
                request.BearerToken,
                "application/json")) // contentType  
            .ReturnsAsync(expectedResponse);

        // Act  
        var result = await _safApiService.UploadMemberPublicKey(request);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal("key-id", result.KeyId);
        Assert.Equal("public-key", result.Key);
    }

    [Fact]
    public async Task UploadMemberPublicKey_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var request = new SafMemberPublicKeyUploadRequest
        {
            BearerToken = "test-bearer-token",
            Payload = new SafMemberPublicKeyUploadRequestPayload
            {
                Key = "public-key",
                Version = "1.0",
                ExpireInDays = "14"
            }
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafMemberPublicKeyUploadRequest, SafMemberPublicKeyResponse>(
                SafDriverConstant.UploadMemberPublicKeyEndpoint,
                request,
                null,
                request.BearerToken, 
                "application/json"))
            .ThrowsAsync(new Exception("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _safApiService.UploadMemberPublicKey(request));
    }
}