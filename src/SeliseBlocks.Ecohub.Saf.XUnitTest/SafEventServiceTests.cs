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
                    PublicKey = "public-key"
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
            PrivateKey = "private-key"
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
                    PublicKey = "public-key"
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