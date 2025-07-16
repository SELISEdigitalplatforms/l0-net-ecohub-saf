using System.ComponentModel.DataAnnotations;
using Moq;
using SeliseBlocks.Ecohub.Saf.Helpers;
using SeliseBlocks.Ecohub.Saf.Services;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class SafApiServiceTests
{
    private readonly Mock<IHttpRequestGateway> _httpRequestGatewayMock;
    private readonly SafGeneralApiService _safApiService;

    public SafApiServiceTests()
    {
        _httpRequestGatewayMock = new Mock<IHttpRequestGateway>();
        _safApiService = new SafGeneralApiService(_httpRequestGatewayMock.Object);
    }

    [Fact]
    public async Task GetReceiversAsync_ShouldReturnReceivers_WhenRequestIsValid()
    {
        // Arrange
        var request = new SafReceiversRequest
        {
            BearerToken = "token",
            Payload = new SafReceiversRequestPayload { LicenceKey = "lk", Password = "pw" }
        };
        var expectedReceivers = new List<SafReceiver> { new SafReceiver { CompanyName = "Test" } };
        var safBaseResponse = new SafBaseResponse<IEnumerable<SafReceiver>> { IsSuccess = true, Data = expectedReceivers };

        _httpRequestGatewayMock
    .Setup(x => x.PostAsync<SafReceiversRequestPayload, IEnumerable<SafReceiver>>(
        It.IsAny<string>(),
        It.IsAny<SafReceiversRequestPayload>(),
        It.IsAny<Dictionary<string, string>>(),
        It.IsAny<string>(),
        It.IsAny<string>()))
    .ReturnsAsync(safBaseResponse);

        // Act
        var result = await _safApiService.GetReceiversAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal("Test", result.Data.First().CompanyName);
    }

    [Fact]
    public async Task GetReceiversAsync_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var request = new SafReceiversRequest(); // Missing required fields

        // Act
        var result = await _safApiService.GetReceiversAsync(request);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("ValidationError", result.Error.ErrorCode);
    }

    // [Fact]
    // public async Task GetMemberPublicKey_ShouldReturnPublicKey_WhenRequestIsValid()
    // {
    //     // Arrange
    //     var bearerToken = "token";
    //     var idpNumber = "idp";
    //     var safBaseResponse = new SafBaseResponse<SafMemberPublicKey>
    //     {
    //         IsSuccess = true,
    //         Data = new SafMemberPublicKey { KeyId = "kid", Key = "pubkey" }
    //     };

    //     _httpRequestGatewayMock
    //         .Setup(x => x.GetAsync<SafMemberPublicKey>(
    //             It.IsAny<string>(), null, bearerToken))
    //         .ReturnsAsync(safBaseResponse);

    //     // Act
    //     var result = await _safApiService.GetMemberPublicKey(bearerToken, idpNumber);

    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal("kid", result.Data.KeyId);
    //     Assert.Equal("pubkey", result.Data.Key);
    // }

    // [Fact]
    // public async Task GetMemberPublicKey_ShouldReturnError_WhenBearerTokenIsNull()
    // {
    //     // Arrange
    //     var idpNumber = "idp";

    //     // Act
    //     var result = await _safApiService.GetMemberPublicKey(null, idpNumber);

    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     Assert.Equal("ValidationError", result.Error.ErrorCode);
    // }

    // [Fact]
    // public async Task UploadMemberPublicKey_ShouldReturnResponse_WhenRequestIsValid()
    // {
    //     // Arrange
    //     var request = new SafMemberPublicKeyUploadRequest
    //     {
    //         BearerToken = "token",
    //         Payload = new SafMemberPublicKeyUploadRequestPayload { Key = "pubkey", Version = "v1", ExpireInDays = "7" }
    //     };
    //     var safBaseResponse = new SafBaseResponse<SafMemberPublicKey>
    //     {
    //         IsSuccess = true,
    //         Data = new SafMemberPublicKey { KeyId = "kid", Key = "pubkey" }
    //     };

    //     _httpRequestGatewayMock
    //         .Setup(x => x.PostAsync<SafMemberPublicKeyUploadRequestPayload, SafMemberPublicKey>(
    //             It.IsAny<string>(),
    //             It.IsAny<SafMemberPublicKeyUploadRequestPayload>(),
    //             It.IsAny<Dictionary<string, string>>(),
    //             It.IsAny<string>(),
    //             It.IsAny<string>()))
    //         .ReturnsAsync(safBaseResponse);
    //     // Act
    //     var result = await _safApiService.UploadMemberPublicKey(request);

    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal("kid", result.Data.KeyId);
    // }

    // [Fact]
    // public async Task UploadMemberPublicKey_ShouldReturnError_WhenValidationFails()
    // {
    //     // Arrange
    //     var request = new SafMemberPublicKeyUploadRequest(); // Missing required fields

    //     // Act
    //     var result = await _safApiService.UploadMemberPublicKey(request);

    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     Assert.Equal("ValidationError", result.Error.ErrorCode);
    // }

    // [Fact]
    // public async Task GetMemberEncryptedPublicKey_ShouldReturnEncryptedKey_WhenRequestIsValid()
    // {
    //     // Arrange
    //     var bearerToken = "token";
    //     var keyId = "kid";
    //     var safBaseResponse = new SafBaseResponse<SafMemberGetEncryptedKey>
    //     {
    //         IsSuccess = true,
    //         Data = new SafMemberGetEncryptedKey { KeyId = keyId, encryptedContent = "enc" }
    //     };

    //     _httpRequestGatewayMock
    //         .Setup(x => x.GetAsync<SafMemberGetEncryptedKey>(
    //             It.IsAny<string>(), null, bearerToken))
    //         .ReturnsAsync(safBaseResponse);

    //     // Act
    //     var result = await _safApiService.GetMemberEncryptedPublicKey(bearerToken, keyId);

    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal(keyId, result.Data.KeyId);
    //     Assert.Equal("enc", result.Data.encryptedContent);
    // }

    // [Fact]
    // public async Task GetMemberEncryptedPublicKey_ShouldReturnError_WhenBearerTokenIsNull()
    // {
    //     // Arrange
    //     var keyId = "kid";

    //     // Act
    //     var result = await _safApiService.GetMemberEncryptedPublicKey(null, keyId);

    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     Assert.Equal("ValidationError", result.Error.ErrorCode);
    // }

    // [Fact]
    // public async Task VerifyMemberDecryptedPublicKey_ShouldReturnSuccess_WhenRequestIsValid()
    // {
    //     // Arrange
    //     var request = new SafMemberVerifyDecryptedKeyRequest
    //     {
    //         BearerToken = "token",
    //         KeyId = "kid",
    //         Payload = new SafMemberVerifyDecryptedKeyRequestPayload { DecryptedContent = "dec" }
    //     };
    //     var safBaseResponse = new SafBaseResponse<SafMemberVerifyDecryptedKey>
    //     {
    //         IsSuccess = true,
    //         Data = new SafMemberVerifyDecryptedKey { VerificationStatus = VerificationStatus.Success }
    //     };

    //     _httpRequestGatewayMock
    //         .Setup(x => x.PostAsync<SafMemberVerifyDecryptedKeyRequest, SafMemberVerifyDecryptedKey>(
    //             It.IsAny<string>(),
    //             It.IsAny<SafMemberVerifyDecryptedKeyRequest>(),
    //             It.IsAny<Dictionary<string, string>>(),
    //             It.IsAny<string>(),
    //             It.IsAny<string>()))
    //         .ReturnsAsync(safBaseResponse);

    //     // Act
    //     var result = await _safApiService.VerifyMemberDecryptedPublicKey(request);

    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal(VerificationStatus.Success, result.Data.VerificationStatus);
    // }

    // [Fact]
    // public async Task VerifyMemberDecryptedPublicKey_ShouldReturnError_WhenValidationFails()
    // {
    //     // Arrange
    //     var request = new SafMemberVerifyDecryptedKeyRequest(); // Missing required fields

    //     // Act
    //     var result = await _safApiService.VerifyMemberDecryptedPublicKey(request);

    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     Assert.Equal("ValidationError", result.Error.ErrorCode);
    // }

    // [Fact]
    // public async Task VerifyMemberDecryptedPublicKey_ShouldReturnError_WhenGatewayThrows()
    // {
    //     // Arrange
    //     var request = new SafMemberVerifyDecryptedKeyRequest
    //     {
    //         BearerToken = "token",
    //         KeyId = "kid",
    //         Payload = new SafMemberVerifyDecryptedKeyRequestPayload { DecryptedContent = "dec" }
    //     };

    //     _httpRequestGatewayMock
    //         .Setup(x => x.PostAsync<SafMemberVerifyDecryptedKeyRequest, SafMemberVerifyDecryptedKey>(
    //             It.IsAny<string>(), request, null, request.BearerToken, null))
    //         .ThrowsAsync(new Exception("Request failed"));

    //     // Act
    //     var result = await _safApiService.VerifyMemberDecryptedPublicKey(request);

    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     Assert.NotNull(result.Error);
    //     Assert.Equal("Failed", result.Error.ErrorCode);
    // }

    // [Fact]
    // public async Task ActivateMemberPublicKey_ShouldReturnSuccess_WhenRequestIsValid()
    // {
    //     // Arrange
    //     var bearerToken = "token";
    //     var keyId = "kid";
    //     var safBaseResponse = new SafBaseResponse<dynamic>
    //     {
    //         IsSuccess = true,
    //         Data = new { Status = "Success" }
    //     };

    //     _httpRequestGatewayMock
    //         .Setup(x => x.PostAsync<object, dynamic>(
    //             It.IsAny<string>(),
    //             It.IsAny<object>(),
    //             It.IsAny<Dictionary<string, string>>(),
    //             It.IsAny<string>(),
    //             It.IsAny<string>()))
    //         .ReturnsAsync(safBaseResponse);

    //     // Act
    //     var result = await _safApiService.ActivateMemberPublicKey(bearerToken, keyId);

    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.NotNull(result.Data);
    // }

    // [Fact]
    // public async Task ActivateMemberPublicKey_ShouldReturnError_WhenBearerTokenIsNull()
    // {
    //     // Arrange
    //     var keyId = "kid";

    //     // Act
    //     var result = await _safApiService.ActivateMemberPublicKey(null, keyId);

    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     Assert.Equal("ValidationError", result.Error.ErrorCode);
    // }

    // [Fact]
    // public async Task ActivateMemberPublicKey_ShouldReturnError_WhenGatewayThrows()
    // {
    //     // Arrange
    //     var bearerToken = "token";
    //     var keyId = "kid";

    //     _httpRequestGatewayMock
    //         .Setup(x => x.PostAsync<object, dynamic>(
    //             It.IsAny<string>(), null, null, bearerToken, null))
    //         .ThrowsAsync(new Exception("Request failed"));

    //     // Act
    //     var result = await _safApiService.ActivateMemberPublicKey(bearerToken, keyId);

    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     Assert.NotNull(result.Error);
    //     Assert.Equal("Failed", result.Error.ErrorCode);
    // }
}