using SeliseBlocks.Ecohub.Saf.Services;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;
public class SafKafkaEventHandlerTests
{
    [Fact]
    public async Task ProduceEventAsync_ReturnsFalse_OnException()
    {
        // Arrange
        var service = new SafKafkaEventHandler();
        var request = new SafProduceKafkaEventRequest
        {
            KafkaServer = null, // This will cause an exception
            KafkaProducerTopic = "test-topic",
            TechUserCertificate = "invalid-base64", // Invalid certificate
            TechUserPassword = "password",
            SchemaRegistryUrl = "http://localhost:8081",
            SchemaRegistryAuth = "user:pass",
            EventPayload = new SafOfferNlpiEvent { Data = new SafData() }
        };

        // Act
        var result = await service.ProduceEventAsync(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ConsumeEventAsync_ReturnsNull_OnException()
    {
        // Arrange
        var service = new SafKafkaEventHandler();
        var request = new SafConsumeKafkaEventRequest
        {
            KafkaServer = null, // This will cause an exception
            TechUserCertificate = "invalid-base64",
            TechUserPassword = "password",
            GroupId = "group1",
            EcohubId = "ecohub1"
        };

        // Act
        var result = service.ConsumeEvent(request);

        // Assert
        Assert.Null(result);
    }
}
