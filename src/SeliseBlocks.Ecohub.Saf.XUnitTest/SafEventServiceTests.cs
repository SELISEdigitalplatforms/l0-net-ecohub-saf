using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class SafEventServiceTests
{
    private readonly Mock<IHttpRequestGateway> _httpRequestGatewayMock;
    private const string _testPublicKey = "-----BEGIN PRIVATE KEY-----MIIBUwIBADANBgkqhkiG9w0BAQEFAASCAT0wggE5AgEAAkEAu6vD4pfDJLCCygmiqGhnRotEmjx2Dx8edcCjfBAeh4QLQhI8paZpaiJSmSgnFkRjUvb8Dhd/GWzlOaqulY+NIQIDAQABAkAxng0RKIyoc55wqjF+EvRTG1kM6jVQdCrKeR8AGwbnTtb/DWyXsnzcO01Ik5TOY1M6+MqhChl3G8PDSJ46RPCtAiEA3RiaPz2y7TiXa6kh/MBn6oiPCP1ZGK2AtZRLPFCZl2sCIQDZTFDbbDCz9QX3lyRUWtpk7d1mHjFTEFEGXW32V2lsowIgPcfnKi7KdcEvhrT/O0pkf0PjfCaXI+8vnQ2wLE11bbsCIHYY1fEK8cU8K4wOZr45ymwEIsm3KxN70K1m5bZ2d2OFAiAvFNT/gmfDGu5p12+SyNh4xhZscLkMC4PSQKTK9/Krkw==-----END PRIVATE KEY-----";
    private const string _testPrivateKey = "-----BEGIN PUBLIC KEY-----MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBALurw+KXwySwgsoJoqhoZ0aLRJo8dg8fHnXAo3wQHoeEC0ISPKWmaWoiUpkoJxZEY1L2/A4Xfxls5TmqrpWPjSECAwEAAQ==-----END PUBLIC KEY-----";
    private const string _testAesKey = "MIIBUwIBADANBgkqhkiG9w0BAQEFAASCAT0wggE5AgEAAkEAu6vD4pfDJLCCygmiqGhnRotEmjx2Dx8edcCjfBAeh4QLQhI8paZpaiJSmSgnFkRjUvb8Dhd/GWzlOaqulY+NIQIDAQABAkAxng0RKIyoc55wqjF+EvRTG1kM6jVQdCrKeR8AGwbnTtb/DWyXsnzcO01Ik5TOY1M6+MqhChl3G8PDSJ46RPCtAiEA3RiaPz2y7TiXa6kh/MBn6oiPCP1ZGK2AtZRLPFCZl2sCIQDZTFDbbDCz9QX3lyRUWtpk7d1mHjFTEFEGXW32V2lsowIgPcfnKi7KdcEvhrT/O0pkf0PjfCaXI+8vnQ2wLE11bbsCIHYY1fEK8cU8K4wOZr45ymwEIsm3KxN70K1m5bZ2d2OFAiAvFNT/gmfDGu5p12+SyNh4xhZscLkMC4PSQKTK9/Krkw==";
    private readonly SafEventService _safEventService;

    public SafEventServiceTests()
    {
        _httpRequestGatewayMock = new Mock<IHttpRequestGateway>();
        _safEventService = new SafEventService(_httpRequestGatewayMock.Object);
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
    public async Task ReceiveOfferNlpiEventAsync_ShouldReturnEvents_WhenRequestIsSuccessful()
    {
        // Arrange
        var request = new SafReceiveOfferNlpiEventRequest
        {
            BearerToken = "test-bearer-token",
            EcohubId = "test-ecohub-id",
            AutoOffsetReset = "earliest",
            PrivateKey = _testPrivateKey
        };

        var expectedResponse = new List<SafReceiveOfferNlpiEventResponse>
    {
        new SafReceiveOfferNlpiEventResponse
        {
            Value = new SafOfferNlpiEncryptedEvent
            {
                Id = "event-id",
                Source = "source",
                Type = "type",
                Data = new SafEncryptedData
                {
                    Payload = "ssdas",
                    EncryptionKey = _testAesKey,
                    PublicKeyVersion = "1.0"
                }
            }
        }
    };

        _httpRequestGatewayMock
            .Setup(x => x.GetAsync<IEnumerable<SafReceiveOfferNlpiEventResponse>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>(),
                request.BearerToken))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safEventService.ReceiveOfferNlpiEventAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("event-id", result.First().Id);
        Assert.Equal("source", result.First().Source);
        Assert.Equal("type", result.First().Type);
        Assert.NotNull(result.First().Data);
    }
}
