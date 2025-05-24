using SeliseBlocks.Ecohub.Saf;
using Xunit;


namespace SeliseBlocks.Ecohub.Saf.XUnitTest;


public class SafBaseResponseMapperTests
{
    private class TestModel
    {
        public string Value { get; set; }
    }

    private class TestResponse : SafBaseResponse<TestModel> { }

    [Fact]
    public void MapToDerivedResponse_ShouldMapAllProperties()
    {
        // Arrange
        var model = new TestModel { Value = "abc" };
        var baseResponse = new SafBaseResponse<TestModel>
        {
            IsSuccess = true,
            Error = new SafError { ErrorCode = "E", ErrorMessage = "msg" },
            Data = model
        };

        // Act
        var result = baseResponse.MapToDerivedResponse<TestModel, TestResponse>();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("E", result.Error.ErrorCode);
        Assert.Equal("msg", result.Error.ErrorMessage);
        Assert.Equal("abc", result.Data.Value);
    }

    [Fact]
    public void MapToResponse_ShouldMapValidationResponse()
    {
        // Arrange
        var validationResponse = new SafValidationResponse
        {
            IsSuccess = false,
            Error = new SafError { ErrorCode = "ValidationError", ErrorMessage = "Invalid" }
        };

        // Act
        var result = validationResponse.MapToResponse<TestModel, TestResponse>();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("ValidationError", result.Error.ErrorCode);
        Assert.Equal("Invalid", result.Error.ErrorMessage);
        Assert.Null(result.Data);
    }
}