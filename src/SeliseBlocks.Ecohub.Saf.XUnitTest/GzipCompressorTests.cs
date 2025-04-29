using System;
using System.Text;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class GzipCompressorTests
{
    [Fact]
    public void CompressBytes_ShouldCompressData()
    {
        // Arrange
        var originalData = Encoding.UTF8.GetBytes("This is a test string for compression.");

        // Act
        var compressedData = GzipCompressor.CompressBytes(originalData);

        // Assert
        Assert.NotNull(compressedData);
        Assert.NotEmpty(compressedData);
    }

    [Fact]
    public void DecompressToString_ShouldDecompressDataToOriginalString()
    {
        // Arrange
        var originalString = "This is a test string for compression.";
        var originalData = Encoding.UTF8.GetBytes(originalString);
        var compressedData = GzipCompressor.CompressBytes(originalData);

        // Act
        var decompressedString = GzipCompressor.DecompressToString(compressedData);

        // Assert
        Assert.NotNull(decompressedString);
        Assert.Equal(originalString, decompressedString);
    }

    [Fact]
    public void DecompressToBytes_ShouldDecompressDataToOriginalBytes()
    {
        // Arrange
        var originalData = Encoding.UTF8.GetBytes("This is a test string for compression.");
        var compressedData = GzipCompressor.CompressBytes(originalData);

        // Act
        var decompressedData = GzipCompressor.DecompressToBytes(compressedData);

        // Assert
        Assert.NotNull(decompressedData);
        Assert.Equal(originalData.Length, decompressedData.Length);
        Assert.Equal(originalData, decompressedData);
    }

    [Fact]
    public void DecompressToBytes_ShouldThrowException_WhenDataIsInvalid()
    {
        // Arrange
        var invalidData = Encoding.UTF8.GetBytes("Invalid compressed data");

        // Act & Assert
        Assert.Throws<InvalidDataException>(() => GzipCompressor.DecompressToBytes(invalidData));
    }
}