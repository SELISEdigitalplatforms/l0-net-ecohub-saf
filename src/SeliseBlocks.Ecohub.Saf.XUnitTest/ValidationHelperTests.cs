using System.ComponentModel.DataAnnotations;
using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class ValidationHelperTests
{
    private class TestModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Range(1, 100, ErrorMessage = "Age must be between 1 and 100")]
        public int Age { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }

    [Fact]
    public void Validate_ShouldNotThrow_WhenModelIsValid()
    {
        // Arrange
        var model = new TestModel
        {
            Name = "John Doe",
            Age = 25,
            Email = "john@example.com"
        };

        // Act & Assert
        var exception = Record.Exception(() => model.Validate());
        Assert.Null(exception);
    }

    [Fact]
    public void Validate_ShouldThrow_WhenModelIsNull()
    {
        // Arrange
        TestModel model = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => model.Validate());
    }

    [Fact]
    public void Validate_ShouldReturnValidationError_WhenRequiredPropertyIsMissing()
    {
        // Arrange
        var model = new TestModel
        {
            Age = 25,
            Email = "john@example.com"
        };

        // Act & Assert
        var errorResponse = model.Validate();
        Assert.Contains("Name is required", errorResponse.Error.ErrorMessage);
    }

    [Fact]
    public void Validate_ShouldReturnValidationError_WhenRangeValidationFails()
    {
        // Arrange
        var model = new TestModel
        {
            Name = "John Doe",
            Age = 101,
            Email = "john@example.com"
        };

        // Act & Assert
        var errorResponse = model.Validate();
        Assert.Contains("Age must be between 1 and 100", errorResponse.Error.ErrorMessage);
    }

    [Fact]
    public void Validate_ShouldReturnValidationError_WhenEmailFormatIsInvalid()
    {
        // Arrange
        var model = new TestModel
        {
            Name = "John Doe",
            Age = 25,
            Email = "invalid-email"
        };

        // Act & Assert
        var errorResponse = model.Validate();
        Assert.Contains("Invalid email format", errorResponse.Error.ErrorMessage);
    }

    [Fact]
    public void Validate_ShouldReturnValidationError_WhenMultipleValidationsFail()
    {
        // Arrange
        var model = new TestModel
        {
            Age = 101,
            Email = "invalid-email"
        };

        // Act & Assert
        var errorResponse = model.Validate();
        Assert.Contains("Name is required", errorResponse.Error.ErrorMessage);
        Assert.Contains("Age must be between 1 and 100", errorResponse.Error.ErrorMessage);
        Assert.Contains("Invalid email format", errorResponse.Error.ErrorMessage);
    }
}
