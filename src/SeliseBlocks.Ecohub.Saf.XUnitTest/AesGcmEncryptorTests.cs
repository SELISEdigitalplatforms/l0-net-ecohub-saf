using System;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class AesGcmEncryptorTests
{
    [Fact]
    public void Encrypt_ShouldReturnBase64EncodedString_WhenInputIsValid()
    {
        // Arrange
        var key = new byte[32]; // 256-bit key
        RandomNumberGenerator.Fill(key);
        var payload = Encoding.UTF8.GetBytes("This is a test payload.");

        // Act
        var encryptedData = AesGcmEncryptor.Encrypt(payload, key);

        // Assert
        Assert.NotNull(encryptedData);
        Assert.NotEmpty(encryptedData);
        Assert.True(Convert.TryFromBase64String(encryptedData, new Span<byte>(new byte[encryptedData.Length]), out _), "Encrypted data should be Base64 encoded.");
    }

    [Fact]
    public void Decrypt_ShouldReturnOriginalPayload_WhenInputIsValid()
    {
        // Arrange
        var key = new byte[32]; // 256-bit key
        RandomNumberGenerator.Fill(key);
        var originalPayload = Encoding.UTF8.GetBytes("This is a test payload.");
        var encryptedData = AesGcmEncryptor.Encrypt(originalPayload, key);

        // Act
        var decryptedPayload = AesGcmEncryptor.Decrypt(encryptedData, key);

        // Assert
        Assert.NotNull(decryptedPayload);
        Assert.Equal(originalPayload, decryptedPayload);
    }

    [Fact]
    public void Encrypt_ShouldThrowArgumentException_WhenKeyIsInvalid()
    {
        // Arrange
        var invalidKey = new byte[16]; // 128-bit key (invalid for AES-GCM)
        var payload = Encoding.UTF8.GetBytes("This is a test payload.");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AesGcmEncryptor.Encrypt(payload, invalidKey));
    }
}