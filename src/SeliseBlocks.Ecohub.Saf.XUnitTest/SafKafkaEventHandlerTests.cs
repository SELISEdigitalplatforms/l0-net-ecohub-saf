using SeliseBlocks.Ecohub.Saf.Services;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class SafKafkaEventHandlerTests
{
    private static readonly Random random = new();
    [Fact]
    public async Task ProduceEventAsync_ReturnsValidationError_WhenRequestIsInvalid()
    {
        // Arrange
        var service = new SafKafkaEventHandler();
        var request = new SafProduceKafkaEventRequest(); // Missing required fields

        // Act
        var result = await service.ProduceEventAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("ValidationError", result.Error.ErrorCode);
    }

    [Fact]
    public async Task ProduceEventAsync_ReturnsError_OnException()
    {
        // Arrange
        var service = new SafKafkaEventHandler();
        var request = new SafProduceKafkaEventRequest
        {
            KafkaServer = null, // This will cause an exception
            KafkaProducerTopic = "test-topic",
            TechUserCertificate = "invalid-base64", // Invalid certificate
            TechUserPassword = RandomString(7),
            SchemaRegistryUrl = "http://localhost:8081",
            SchemaRegistryAuth = "user:pass",
            EventPayload = new SafOfferNlpiEvent { Data = new SafData() }
        };

        // Act
        var result = await service.ProduceEventAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("ValidationError", result.Error.ErrorCode);
    }

    [Fact]
    public void ConsumeEvent_ReturnsError_OnException()
    {
        // Arrange
        var service = new SafKafkaEventHandler();
        var request = new SafConsumeKafkaEventRequest
        {
            KafkaServer = null, // This will cause an exception
            TechUserCertificate = "invalid-base64",
            TechUserPassword = RandomString(7),
            GroupId = "group1",
            EcohubId = "ecohub1"
        };

        // Act
        var result = service.ConsumeEvent(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal("Exception", result.Error.ErrorCode);
        Assert.Contains("The input is not a valid Base-64 string", result.Error.ErrorMessage);
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}