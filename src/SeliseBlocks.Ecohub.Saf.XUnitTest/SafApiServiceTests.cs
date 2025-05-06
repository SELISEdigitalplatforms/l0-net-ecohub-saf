using System.ComponentModel.DataAnnotations;
using Moq;
using SeliseBlocks.Ecohub.Saf.Helpers;
using SeliseBlocks.Ecohub.Saf.Services;

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
            .Setup(x => x.PostAsync<SafMemberPublicKeyUploadRequestPayload, SafMemberPublicKeyResponse>(
                SafDriverConstant.UploadMemberPublicKeyEndpoint,
                request.Payload,
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
            .Setup(x => x.PostAsync<SafMemberPublicKeyUploadRequestPayload, SafMemberPublicKeyResponse>(
                SafDriverConstant.UploadMemberPublicKeyEndpoint,
                request.Payload,
                null,
                request.BearerToken, 
                "application/json"))
            .ThrowsAsync(new Exception("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _safApiService.UploadMemberPublicKey(request));
    }

    [Fact]
    public async Task GetMemberEncryptedPublicKey_ShouldReturnEncryptedKey_WhenRequestIsSuccessful()
    {
        // Arrange
        var bearerToken = "test-bearer-token";
        var keyId = "key-id-123";
        var expectedResponse = new SafMemberGetEncryptedKeyResponse
        {
            KeyId = keyId,
            encryptedContent = "encrypted-public-key-content"
        };

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<SafMemberGetEncryptedKeyResponse>(
                It.Is<string>(url => url.Contains(keyId)),
                null,
                bearerToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safApiService.GetMemberEncryptedPublicKey(bearerToken, keyId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(keyId, result.KeyId);
        Assert.Equal("encrypted-public-key-content", result.encryptedContent);
    }

    [Fact]
    public async Task GetMemberEncryptedPublicKey_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var bearerToken = "test-bearer-token";
        var keyId = "key-id-123";

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<SafMemberGetEncryptedKeyResponse>(
                It.Is<string>(url => url.Contains(keyId)),
                null,
                bearerToken))
            .ThrowsAsync(new Exception("Request failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _safApiService.GetMemberEncryptedPublicKey(bearerToken, keyId));
    }

    [Fact]
    public async Task VerifyMemberDecryptedPublicKey_ShouldReturnSuccess_WhenRequestIsSuccessful()
    {
        // Arrange  
        var request = new SafMemberVerifyDecryptedKeyRequest
        {
            BearerToken = "test-bearer-token",
            KeyId = "key-id-123",
            Payload = new SafMemberVerifyDecryptedKeyRequestPayload() { DecryptedContent = "test-decripted_content" }
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafMemberVerifyDecryptedKeyRequest, SafMemberVerifyDecryptedKeyResponse>(
                It.Is<string>(url => url.Contains(request.KeyId)),
                request,
                null,
                request.BearerToken,
                "application/json"))
            .ReturnsAsync(new SafMemberVerifyDecryptedKeyResponse
            {
                VerificationStatus = VerificationStatus.Success
            });

        // Act  
        var result = await _safApiService.VerifyMemberDecryptedPublicKey(request);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(VerificationStatus.Success, result.VerificationStatus);
    }

    [Fact]
    public async Task VerifyMemberDecryptedPublicKey_ShouldReturnFail_WhenRequestThrowsException()
    {
        // Arrange  
        var request = new SafMemberVerifyDecryptedKeyRequest
        {
            BearerToken = "test-bearer-token",
            KeyId = "key-id-123",
            Payload = new SafMemberVerifyDecryptedKeyRequestPayload() { DecryptedContent = "test-decripted_content" }
        };
        

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafMemberVerifyDecryptedKeyRequest, SafMemberVerifyDecryptedKeyResponse>(
                It.Is<string>(url => url.Contains(request.KeyId)),
                request,
                null,
                request.BearerToken,
                "application/json"))
            .ThrowsAsync(new Exception("Request failed"));

        // Act  
        var result = await _safApiService.VerifyMemberDecryptedPublicKey(request);

        // Assert  
        Assert.NotNull(result);
        Assert.Equal(VerificationStatus.Fail, result.VerificationStatus);
    }

    [Fact]
    public async Task VerifyMemberDecryptedPublicKey_ShouldThrowArgumentException_WhenBearerTokenIsNull()
    {
        // Arrange
        var request = new SafMemberVerifyDecryptedKeyRequest
        {
            BearerToken = null,
            KeyId = "key-id-123",
            Payload = new SafMemberVerifyDecryptedKeyRequestPayload()
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _safApiService.VerifyMemberDecryptedPublicKey(request));
        Assert.Equal("Invalid person request: BearerToken is required", exception.Message);
    }

    [Fact]
    public async Task VerifyMemberDecryptedPublicKey_ShouldThrowArgumentException_WhenKeyIdIsNull()
    {
        // Arrange
        var request = new SafMemberVerifyDecryptedKeyRequest
        {
            BearerToken = "test-bearer-token",
            KeyId = null,
            Payload = new SafMemberVerifyDecryptedKeyRequestPayload() { DecryptedContent = "test-decripted_content" }
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _safApiService.VerifyMemberDecryptedPublicKey(request));
        Assert.Equal("Invalid person request: KeyId is required", exception.Message);
    }

    [Fact]
    public async Task ActivateMemberPublicKey_ShouldReturnTrue_WhenRequestIsSuccessful()
    {
        // Arrange
        var bearerToken = "test-bearer-token";
        var keyId = "key-id-123";

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<object, object>(
                It.Is<string>(url => url.Contains(keyId)),
                null,
                null,
                bearerToken,  
                "application/json"))
            .ReturnsAsync(new object());

        var result = await _safApiService.ActivateMemberPublicKey(bearerToken, keyId);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task ActivateMemberPublicKey_ShouldReturnFalse_WhenRequestThrowsException()
    {
        // Arrange
        var bearerToken = "test-bearer-token";
        var keyId = "key-id-123";

        _httpRequestGatewayMock
           .Setup(x => x.PostAsync<object, object>(
               It.Is<string>(url => url.Contains(keyId)),
               null,
               null,
               bearerToken,
               "application/json")) 
           .ThrowsAsync(new Exception("Request failed"));

        // Act
        var result = await _safApiService.ActivateMemberPublicKey(bearerToken, keyId);

        // Assert
        Assert.False(result);
    }
    [Fact]
    public async Task ActivateMemberPublicKey_ShouldThrowArgumentException_WhenBearerTokenIsNull()
    {
        // Arrange
        string bearerToken = null;
        var keyId = "key-id-123";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _safApiService.ActivateMemberPublicKey(bearerToken, keyId));
        Assert.Equal("bearerToken cannot be null or empty. (Parameter 'bearerToken')", exception.Message);
    }

    [Fact]
    public async Task ActivateMemberPublicKey_ShouldThrowArgumentException_WhenKeyIdIsNull()
    {
        // Arrange
        var bearerToken = "test-bearer-token";
        string keyId = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _safApiService.ActivateMemberPublicKey(bearerToken, keyId));
        Assert.Equal("keyId cannot be null or empty. (Parameter 'keyId')", exception.Message);
    }
}