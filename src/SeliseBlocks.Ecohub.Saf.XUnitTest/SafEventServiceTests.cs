using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class SafEventServiceTests
{
    private readonly Mock<IHttpRequestGateway> _httpRequestGatewayMock;
    private readonly SafEventService _safEventService;

    private const string _testPrivateKey = "-----BEGIN RSA PRIVATE KEY-----\r\nMIIBOgIBAAJBAKj34GkxFhD90vcNLYLInFEX6Ppy1tPf9Cnzj4p4WGeKLs1Pt8Qu\r\nKUpRKfFLfRYC9AIKjbJTWit+CqvjWYzvQwECAwEAAQJAIJLixBy2qpFoS4DSmoEm\r\no3qGy0t6z09AIJtH+5OeRV1be+N4cDYJKffGzDa88vQENZiRm0GRq6a+HPGQMd2k\r\nTQIhAKMSvzIBnni7ot/OSie2TmJLY4SwTQAevXysE2RbFDYdAiEBCUEaRQnMnbp7\r\n9mxDXDf6AU0cN/RPBjb9qSHDcWZHGzUCIG2Es59z8ugGrDY+pxLQnwfotadxd+Uy\r\nv/Ow5T0q5gIJAiEAyS4RaI9YG8EWx/2w0T67ZUVAw8eOMB6BIUg0Xcu+3okCIBOs\r\n/5OiPgoTdSy7bcF9IGpSE8ZgGKzgYQVZeN97YE00\r\n-----END RSA PRIVATE KEY-----";
    private const string _testPublicKey = "-----BEGIN RSA PUBLIC KEY-----\r\nMIIBCgKCAQEA+xGZ/wcz9ugFpP07Nspo6U17l0YhFiFpxxU4pTk3Lifz9R3zsIsu\r\nERwta7+fWIfxOo208ett/jhskiVodSEt3QBGh4XBipyWopKwZ93HHaDVZAALi/2A\r\n+xTBtWdEo7XGUujKDvC2/aZKukfjpOiUI8AhLAfjmlcD/UZ1QPh0mHsglRNCmpCw\r\nmwSXA9VNmhz+PiB+Dml4WWnKW/VHo2ujTXxq7+efMU4H2fny3Se3KYOsFPFGZ1TN\r\nQSYlFuShWrHPtiLmUdPoP6CV2mML1tk+l7DIIqXrQhLUKDACeM5roMx0kLhUWB8P\r\n+0uj1CNlNN4JRZlC7xFfqiMbFRU9Z4N6YwIDAQAB\r\n-----END RSA PUBLIC KEY-----";

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
            .Setup(x => x.PostAsync<SafOfferNlpiEvent, SafSendOfferNlpiEventResponse>(
                It.IsAny<string>(),
                request.EventPayload,
                null,
                request.BearerToken,
                "application/json"))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safEventService.SendOfferNlpiEventAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.KeySchemaId);
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

        var expectedResponse = new List<SafOfferNlpiEvent>
        {
            new SafOfferNlpiEvent
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

        _httpRequestGatewayMock
            .Setup(x => x.PostAsync<SafReceiveOfferNlpiEventRequest, IEnumerable<SafOfferNlpiEvent>>(
                It.IsAny<string>(),
                request,
                null,
                request.BearerToken,
                "application/json"))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _safEventService.ReceiveOfferNlpiEventAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("event-id", result.First().Id);
    }
}