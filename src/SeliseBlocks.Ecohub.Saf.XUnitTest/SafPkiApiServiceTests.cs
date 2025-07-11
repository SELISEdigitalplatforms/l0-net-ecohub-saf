using Moq;
using SeliseBlocks.Ecohub.Saf.Services;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class SafPkiApiServiceTests
{
    private readonly Mock<IHttpRequestGateway> _httpRequestGatewayMock;
    private readonly SafPkiApiService _service;

    public SafPkiApiServiceTests()
    {
        _httpRequestGatewayMock = new Mock<IHttpRequestGateway>();
        _service = new SafPkiApiService(_httpRequestGatewayMock.Object);
    }

    [Fact]
    public async Task UploadMemberPublicKey_ReturnsValidationError_WhenRequestIsInvalid()
    {
        var request = new SafMemberPublicKeyUploadRequest(); // missing required fields
        var result = await _service.UploadMemberPublicKey(request);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task UploadMemberPublicKey_ReturnsSuccess_WhenRequestIsValid()
    {
        var request = new SafMemberPublicKeyUploadRequest
        {
            BearerToken = "token",
            Payload = new List<SafMemberPublicKeyUploadRequestPayload> {
                new SafMemberPublicKeyUploadRequestPayload { Key = "key", Version = "v1", ExpireInDays = "365" }
            }
        };
        var safBaseResponse = new SafBaseResponse<IEnumerable<SafMemberPublicKey>>
        {
            IsSuccess = true,
            Data = new List<SafMemberPublicKey> { new SafMemberPublicKey { KeyId = "kid", Key = "key" } }
        };
        _httpRequestGatewayMock.Setup(x => x.PostAsync<IEnumerable<SafMemberPublicKeyUploadRequestPayload>, IEnumerable<SafMemberPublicKey>>(
            It.IsAny<string>(), It.IsAny<IEnumerable<SafMemberPublicKeyUploadRequestPayload>>(), null, request.BearerToken, null))
            .ReturnsAsync(safBaseResponse);
        var result = await _service.UploadMemberPublicKey(request);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetMemberPublicKey_ReturnsValidationError_WhenTokenOrIdpIsNull()
    {
        var result1 = await _service.GetMemberPublicKey(null, "idp");
        Assert.False(result1.IsSuccess);
        Assert.NotNull(result1.Error);
        var result2 = await _service.GetMemberPublicKey("token", null);
        Assert.False(result2.IsSuccess);
        Assert.NotNull(result2.Error);
    }

    [Fact]
    public async Task GetMemberPublicKey_ReturnsSuccess_WhenValid()
    {
        var safBaseResponse = new SafBaseResponse<IEnumerable<SafMemberPublicKey>>
        {
            IsSuccess = true,
            Data = new List<SafMemberPublicKey> { new SafMemberPublicKey { KeyId = "kid", Key = "key" } }
        };
        _httpRequestGatewayMock.Setup(x => x.GetAsync<IEnumerable<SafMemberPublicKey>>(
            It.IsAny<string>(), null, "token"))
            .ReturnsAsync(safBaseResponse);
        var result = await _service.GetMemberPublicKey("token", "idp");
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetMemberEncryptedPublicKey_ReturnsValidationError_WhenTokenOrKeyIdIsNull()
    {
        var result1 = await _service.GetMemberEncryptedPublicKey(null, "keyid");
        Assert.False(result1.IsSuccess);
        Assert.NotNull(result1.Error);
        var result2 = await _service.GetMemberEncryptedPublicKey("token", null);
        Assert.False(result2.IsSuccess);
        Assert.NotNull(result2.Error);
    }

    [Fact]
    public async Task GetMemberEncryptedPublicKey_ReturnsSuccess_WhenValid()
    {
        var safBaseResponse = new SafBaseResponse<SafMemberGetEncryptedKey>
        {
            IsSuccess = true,
            Data = new SafMemberGetEncryptedKey { KeyId = "kid", VerificationContent = "enc", KeyType = "encryption" }
        };
        _httpRequestGatewayMock.Setup(x => x.GetAsync<SafMemberGetEncryptedKey>(
            It.IsAny<string>(), null, "token"))
            .ReturnsAsync(safBaseResponse);
        var result = await _service.GetMemberEncryptedPublicKey("token", "keyid");
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task DeactivatePublicKey_ReturnsValidationError_WhenTokenOrKeyIdIsNull()
    {
        var result1 = await _service.DeactivatePublicKey(null, "keyid");
        Assert.False(result1.IsSuccess);
        Assert.NotNull(result1.Error);
        var result2 = await _service.DeactivatePublicKey("token", null);
        Assert.False(result2.IsSuccess);
        Assert.NotNull(result2.Error);
    }

    [Fact]
    public async Task DeleteInactivePublicKey_ReturnsValidationError_WhenTokenOrKeyIdIsNull()
    {
        var result1 = await _service.DeleteInactivePublicKey(null, "keyid");
        Assert.False(result1.IsSuccess);
        Assert.NotNull(result1.Error);
        var result2 = await _service.DeleteInactivePublicKey("token", null);
        Assert.False(result2.IsSuccess);
        Assert.NotNull(result2.Error);
    }

    [Fact]
    public async Task ActivateMemberPublicKey_ReturnsValidationError_WhenTokenOrKeyIdIsNull()
    {
        var result1 = await _service.ActivateMemberPublicKey(null, "keyid");
        Assert.False(result1.IsSuccess);
        Assert.NotNull(result1.Error);
        var result2 = await _service.ActivateMemberPublicKey("token", null);
        Assert.False(result2.IsSuccess);
        Assert.NotNull(result2.Error);
    }
}
