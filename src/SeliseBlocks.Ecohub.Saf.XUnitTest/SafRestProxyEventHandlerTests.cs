using System.ComponentModel.DataAnnotations;
using Moq;
using SeliseBlocks.Ecohub.Saf.Helpers;
using SeliseBlocks.Ecohub.Saf.Services;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class SafRestProxyEventHandlerTests
{
    private readonly Mock<IHttpRequestGateway> _httpRequestGatewayMock;
    private const string _testPublicKey = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAspmx/YwNpbRhxtQWQzWX
KUYe1jmRabnXozNaPFEeiOXdaE3Gvty897sjl3HmBbnEpdvGl0NLbm0J7klVHrjC
AatfTl3UJuJiWunKAhlYiTtR/eyE6jdovQMJzxfJHa+kc7IFjaPrUUs7Ppvmod4C
/zv24pD+BVR7hjJnqM2nbr5vqZwE4qFDXcC6MW+AEDziSY3qBm8ExoirXq3MT6UD
XllwVyjKmBcI79HtNMjVToHZIIo/k/SCLO3pNYRmpSWMRZ6vOrLicBmeaEFjMGFo
UmIzjiQhDZSg37urT2QPBe+YbhKuBrmoNheLGHtoUZpReuQCsN+3iMnVorxkHMmk
mwIDAQAB
-----END PUBLIC KEY-----";

    private const string _testPrivateKey = @"-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCymbH9jA2ltGHG
1BZDNZcpRh7WOZFpudejM1o8UR6I5d1oTca+3Lz3uyOXceYFucSl28aXQ0tubQnu
SVUeuMIBq19OXdQm4mJa6coCGViJO1H97ITqN2i9AwnPF8kdr6RzsgWNo+tRSzs+
m+ah3gL/O/bikP4FVHuGMmeozaduvm+pnATioUNdwLoxb4AQPOJJjeoGbwTGiKte
rcxPpQNeWXBXKMqYFwjv0e00yNVOgdkgij+T9IIs7ek1hGalJYxFnq86suJwGZ5o
QWMwYWhSYjOOJCENlKDfu6tPZA8F75huEq4Guag2F4sYe2hRmlF65AKw37eIydWi
vGQcyaSbAgMBAAECggEAH3HM8yF9459LGbkMhF/Ckes9EaWIEw+7xgmMCROVJzAl
V7Bd3gu6H3mszgSpJXfsBfGYWNhpxvLerTvvBx4rViTofkEp0YDJJU2FGfKBcoPl
rym9ywjfYWvQBcyfxaC/ePkuXh4ul50BvMexBu2yJGLX2FMDzkduChYExyUSJf6J
EYzs9yL6nXrR4Cbmv3716oihyhVpiS1IdH1UMH+R+6tPrFPJoFr86viaR0gN3ejb
fAgBDbb4uTPI8aWb8msB7m8yguSutNv+d+xuV5UR4FVR4IKNwblcc021QJIvQHYw
MuAzynpwfPhceqQth+vKTlalIlkJh6eC5bDjgyXQgQKBgQDcR+GHk4ABmmka6fTf
0t/UPeOYVXELfnVXXMfwDXgFfeY+d8Mvpmf4DIinJo0bL73vMXSqdNybem9DkDQj
fB6I3PMh04GhVwPmY/taVqxzLYJg61QVSZ8EXRvFjsS0bDcUgztoVdGOVB+F1RB7
n8mK7v0CmM9o75VDlLx8cO63GwKBgQDPj5xINxeLbz7U9dZY97FmLpMpBMDKpLKM
HHtzE2Di9YgkeAfKTyz1LX0j72ij97kWzwwQBOx45y01OlFH5YDmEYHalpb6RDBe
ZVHo22f1vsdB8FWMQMGiqdMlsvfQ1zZU+Ban1yxkZft+b3RfTBgOWb1SzkdEE+hK
uGW8ppQggQKBgQCQtHenRHIWm4ToNUCzuCdpma5lZ9t3HX+gAEcnnvF1ShtydeI2
7y3lePZcN6sCbP5snyRwxYwWZvuoepaFqQe2CM9/LR4/CpZ5Rrzbv4xRrVe0q2L1
CQP5LeEMipkVnPEh/IOOKrIauZBrrmfBjlordoumpRO7b4eyeYbIiLeIeQKBgCHg
Blmi5CzVkyOem8UZZ9KNd2cSZ4SrLJjBbURyvTVNbVLGZD8YfPXm3q2mvSVFoOeg
Ew/qPc3drPsq8WkSg98IrHDIcwuVZW+CicO/S1BIOq0AVHX3e6LYpKVaeCeVeECV
3Ny3uX8JRep0tkF3YdW1v7hsAiWSOi83uSL47OQBAoGAXLkcvaVTxlapU4xwjziE
0HEjtFsshmNaXXpaywAgnS88t4lco0cV7MYdkeTlpk01WW/PhrEpldVY+y2W3Jim
BXEJV0nDz043plwyNj6Y+5zvIbfyXnb3orKNoZ9ft9V5vrkj0bWphCaUVQkkov6s
/nSOpaKFyo4MfhPJTSDAJkc=
-----END PRIVATE KEY-----";
    private const string _testAesKey = "MIIBUwIBADANBgkqhkiG9w0BAQEFAASCAT0wggE5AgEAAkEAu6vD4pfDJLCCygmiqGhnRotEmjx2Dx8edcCjfBAeh4QLQhI8paZpaiJSmSgnFkRjUvb8Dhd/GWzlOaqulY+NIQIDAQABAkAxng0RKIyoc55wqjF+EvRTG1kM6jVQdCrKeR8AGwbnTtb/DWyXsnzcO01Ik5TOY1M6+MqhChl3G8PDSJ46RPCtAiEA3RiaPz2y7TiXa6kh/MBn6oiPCP1ZGK2AtZRLPFCZl2sCIQDZTFDbbDCz9QX3lyRUWtpk7d1mHjFTEFEGXW32V2lsowIgPcfnKi7KdcEvhrT/O0pkf0PjfCaXI+8vnQ2wLE11bbsCIHYY1fEK8cU8K4wOZr45ymwEIsm3KxN70K1m5bZ2d2OFAiAvFNT/gmfDGu5p12+SyNh4xhZscLkMC4PSQKTK9/Krkw==";
    private readonly SafRestProxyEventHandler _safEventService;

    public SafRestProxyEventHandlerTests()
    {
        _httpRequestGatewayMock = new Mock<IHttpRequestGateway>();
        _safEventService = new SafRestProxyEventHandler(_httpRequestGatewayMock.Object);
    }

    [Fact]
    public async Task SendOfferNlpiEventAsync_ShouldReturnResponse_WhenRequestIsSuccessful()
    {
        // Arrange
        var request = new SafSendOfferNlpiEventRequest
        {
            SchemaVersionId = "1",
            KeySchemaVersionId = "1",
            BearerToken = "test-bearer-token",
            EventPayload = new SafOfferNlpiEvent
            {
                Id = "event-id",
                Source = "source",
                Type = "type",
                Data = new SafData
                {
                    Payload = new byte[] { 1, 2, 3 },
                    PublicKey = _testPublicKey
                }
            }
        };

        var expectedResponse = new SafSendOfferNlpiEventResponse
        {
            KeySchemaId = 1,
            ValueSchemaId = 1,
            Offsets = new List<SafSendOfferNlpiEventResponseOffset>
            {
                new SafSendOfferNlpiEventResponseOffset
                {
                    Partition = 0,
                    Offset = 1
                }
            }
        };

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafOfferNlpiEncryptedEvent, SafSendOfferNlpiEventResponse>(
                It.IsAny<string>(),
                It.IsAny<SafOfferNlpiEncryptedEvent>(),
                It.IsAny<Dictionary<string, string>>(),
                request.BearerToken, "application/json"))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safEventService.SendOfferNlpiEventAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.KeySchemaId);
        Assert.Equal(1, result.ValueSchemaId);
        Assert.Single(result.Offsets);
        Assert.Equal(0, result.Offsets.First().Partition);
        Assert.Equal(1, result.Offsets.First().Offset);
    }

    [Fact]
    public async Task ReceiveOfferNlpiEventAsync_ShouldReturnEvents_WhenRequestIsValid()
    {
        // Arrange
        var request = new SafReceiveOfferNlpiEventRequest
        {
            BearerToken = "test-bearer-token",
            EcohubId = "test-ecohub-id",
            AutoOffsetReset = "earliest",
            PrivateKey = _testPrivateKey
        };

        // Create test data that will be "encrypted"
        var originalPayload = new byte[] { 1, 2, 3, 4, 5 };
        var aesKey = KmsHelper.GenerateAesKey();
        var compressedData = GzipCompressor.CompressBytes(originalPayload);
        var encryptedData = KmsHelper.EncryptWithAesKey(compressedData, aesKey);
        var encryptedAesKey = KmsHelper.EncryptAesKeyWithPublicKey(aesKey, _testPublicKey);

        var mockResponses = new List<SafReceiveOfferNlpiEventResponse>
    {
        new SafReceiveOfferNlpiEventResponse
        {
            Topic = "test-topic",
            Key = new SafReceiveOfferNlpiEventKey { ProcessId = "test-process" },
            Partition = 0,
            Offset = 1,
            Value = new SafOfferNlpiEncryptedEvent
            {
                Id = "test-id",
                Source = "test-source",
                Type = "test-type",
                Time = DateTime.UtcNow.ToString("o"),
                Data = new SafEncryptedData
                {
                    Payload = encryptedData,
                    EncryptionKey = encryptedAesKey,
                    PublicKeyVersion = "1.0",
                    Message = "Test message",
                    Links = new List<SafLinks>()
                }
            }
        }
    };

        var expectedEndpoint = SafDriverConstant.ReceiveOfferNlpiEventEndpoint
            .Replace("{ecohubId}", request.EcohubId);

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<IEnumerable<SafReceiveOfferNlpiEventResponse>>(
                It.Is<string>(url => url == expectedEndpoint),
                It.Is<Dictionary<string, string>>(h => h["auto.offset.reset"] == request.AutoOffsetReset),
                request.BearerToken))
            .ReturnsAsync(mockResponses);

        // Act
        var result = await _safEventService.ReceiveOfferNlpiEventAsync(request);

        // Assert
        Assert.NotNull(result);
        var eventsList = result.ToList();
        Assert.Single(eventsList);

        var firstEvent = eventsList[0];
        Assert.Equal(mockResponses[0].Value.Id, firstEvent.Id);
        Assert.Equal(mockResponses[0].Value.Source, firstEvent.Source);
        Assert.Equal(mockResponses[0].Value.Type, firstEvent.Type);
        Assert.NotNull(firstEvent.Data);
        Assert.NotNull(firstEvent.Data.Payload);
        Assert.Equal(originalPayload, firstEvent.Data.Payload); // Verify the decrypted payload matches original

        // Verify mock was called with correct parameters
        _httpRequestGatewayMock.Verify(x => x.GetAsync<IEnumerable<SafReceiveOfferNlpiEventResponse>>(
            expectedEndpoint,
            It.Is<Dictionary<string, string>>(h => h["auto.offset.reset"] == request.AutoOffsetReset),
            request.BearerToken),
            Times.Once);
    }

    [Fact]
    public async Task ReceiveOfferNlpiEventAsync_ShouldThrowException_WhenEcohubIdIsEmpty()
    {
        // Arrange
        var request = new SafReceiveOfferNlpiEventRequest
        {
            BearerToken = "test-token",
            EcohubId = string.Empty,
            AutoOffsetReset = "earliest",
            PrivateKey = _testPrivateKey
        };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _safEventService.ReceiveOfferNlpiEventAsync(request));
    }

    [Fact]
    public async Task ReceiveOfferNlpiEventAsync_ShouldReturnEmptyList_WhenNoEventsReceived()
    {
        // Arrange
        var request = new SafReceiveOfferNlpiEventRequest
        {
            BearerToken = "test-token",
            EcohubId = "test-ecohub",
            AutoOffsetReset = "earliest",
            PrivateKey = _testPrivateKey
        };

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<IEnumerable<SafReceiveOfferNlpiEventResponse>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>()))
            .ReturnsAsync(new List<SafReceiveOfferNlpiEventResponse>());

        // Act
        var result = await _safEventService.ReceiveOfferNlpiEventAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

}
