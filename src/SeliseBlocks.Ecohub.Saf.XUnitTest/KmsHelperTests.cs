using System.Security.Cryptography;
using System.Text;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class KmsHelperTests
{
    private readonly string _testPublicKey;
    private readonly string _testPrivateKey;

    public KmsHelperTests()
    {
        using var rsa = RSA.Create(2048);
        _testPublicKey = ExportPublicKey(rsa);
        _testPrivateKey = ExportPrivateKey(rsa);
    }

    private string ExportPublicKey(RSA rsa)
    {
        return $"-----BEGIN PUBLIC KEY-----\n{Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo(), Base64FormattingOptions.InsertLineBreaks)}\n-----END PUBLIC KEY-----";
    }

    private string ExportPrivateKey(RSA rsa)
    {
        return $"-----BEGIN PRIVATE KEY-----\n{Convert.ToBase64String(rsa.ExportPkcs8PrivateKey(), Base64FormattingOptions.InsertLineBreaks)}\n-----END PRIVATE KEY-----";
    }

    [Fact]
    public void GenerateAesKey_ShouldReturn32ByteKey()
    {
        var key = KmsHelper.GenerateAesKey();
        Assert.NotNull(key);
        Assert.Equal(32, key.Length);
    }

    [Fact]
    public void EncryptAesKeyWithPublicKey_ShouldEncryptKey()
    {
        var aesKey = KmsHelper.GenerateAesKey();
        var encryptedKey = KmsHelper.EncryptAesKeyWithPublicKey(aesKey, _testPublicKey);
        Assert.NotNull(encryptedKey);
        Assert.True(Convert.TryFromBase64String(encryptedKey, new byte[encryptedKey.Length], out _));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-pem")]
    public void EncryptAesKeyWithPublicKey_ShouldThrowException_WhenPublicKeyIsInvalid(string invalidKey)
    {
        var aesKey = KmsHelper.GenerateAesKey();
        Assert.ThrowsAny<Exception>(() => KmsHelper.EncryptAesKeyWithPublicKey(aesKey, invalidKey));
    }

    [Fact]
    public void EncryptAesKeyWithPublicKey_ShouldThrowException_WhenAesKeyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            KmsHelper.EncryptAesKeyWithPublicKey(null, _testPublicKey));
    }

    [Fact]
    public void DecryptAesKeyWithPrivateKey_ShouldDecryptKey()
    {
        var originalKey = KmsHelper.GenerateAesKey();
        var encryptedKey = KmsHelper.EncryptAesKeyWithPublicKey(originalKey, _testPublicKey);
        var decryptedKey = KmsHelper.DecryptAesKeyWithPrivateKey(encryptedKey, _testPrivateKey);
        Assert.Equal(originalKey, decryptedKey);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-pem")]
    public void DecryptAesKeyWithPrivateKey_ShouldThrowException_WhenPrivateKeyIsInvalid(string invalidKey)
    {
        var encryptedKey = Convert.ToBase64String(new byte[] { 1, 2, 3 });
        Assert.ThrowsAny<Exception>(() =>
            KmsHelper.DecryptAesKeyWithPrivateKey(encryptedKey, invalidKey));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-base64")]
    public void DecryptAesKeyWithPrivateKey_ShouldThrowException_WhenEncryptedKeyIsInvalid(string invalidKey)
    {
        Assert.ThrowsAny<Exception>(() =>
            KmsHelper.DecryptAesKeyWithPrivateKey(invalidKey, _testPrivateKey));
    }

    [Fact]
    public void EncryptWithAesKey_ShouldEncryptData()
    {
        var key = KmsHelper.GenerateAesKey();
        var data = Encoding.UTF8.GetBytes("Test data");
        var encryptedData = KmsHelper.EncryptWithAesKey(data, key);
        Assert.NotNull(encryptedData);
        Assert.True(Convert.TryFromBase64String(encryptedData, new byte[encryptedData.Length], out _));
    }

    [Fact]
    public void EncryptWithAesKey_ShouldThrowException_WhenPayloadIsNull()
    {
        var key = KmsHelper.GenerateAesKey();
        Assert.Throws<Exception>(() => KmsHelper.EncryptWithAesKey(null, key));
    }

    [Fact]
    public void EncryptWithAesKey_ShouldThrowException_WhenKeyIsNull()
    {
        var data = Encoding.UTF8.GetBytes("Test data");
        Assert.Throws<Exception>(() => KmsHelper.EncryptWithAesKey(data, null));
    }

    [Fact]
    public void EncryptWithAesKey_ShouldThrowException_WhenKeyLengthIsInvalid()
    {
        var data = Encoding.UTF8.GetBytes("Test data");
        var invalidKey = new byte[16];
        Assert.Throws<ArgumentException>(() => KmsHelper.EncryptWithAesKey(data, invalidKey));
    }

    [Fact]
    public void DecryptWithAesKey_ShouldDecryptData()
    {
        var key = KmsHelper.GenerateAesKey();
        var originalData = Encoding.UTF8.GetBytes("Test data");
        var encryptedData = KmsHelper.EncryptWithAesKey(originalData, key);
        var decryptedData = KmsHelper.DecryptWithAesKey(encryptedData, key);
        Assert.Equal(originalData, decryptedData);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-base64")]
    public void DecryptWithAesKey_ShouldThrowException_WhenBase64DataIsInvalid(string invalidData)
    {
        var key = KmsHelper.GenerateAesKey();
        Assert.ThrowsAny<Exception>(() => KmsHelper.DecryptWithAesKey(invalidData, key));
    }

    [Fact]
    public void DecryptWithAesKey_ShouldThrowException_WhenKeyIsNull()
    {
        var encryptedData = Convert.ToBase64String(new byte[] { 1, 2, 3 });
        Assert.Throws<ArgumentException>(() => KmsHelper.DecryptWithAesKey(encryptedData, null));
    }

    [Fact]
    public void DecryptWithAesKey_ShouldThrowException_WhenKeyLengthIsInvalid()
    {
        var encryptedData = Convert.ToBase64String(new byte[] { 1, 2, 3 });
        var invalidKey = new byte[16];
        Assert.Throws<ArgumentException>(() => KmsHelper.DecryptWithAesKey(encryptedData, invalidKey));
    }

    [Fact]
    public void DecryptWithAesKey_ShouldThrowException_WhenAuthenticationFails()
    {
        var key = KmsHelper.GenerateAesKey();
        var data = Encoding.UTF8.GetBytes("Test data");
        var encryptedData = KmsHelper.EncryptWithAesKey(data, key);
        var tamperedData = encryptedData.Substring(0, encryptedData.Length - 4) + "AAAA";

        Assert.Throws<CryptographicException>(() =>
            KmsHelper.DecryptWithAesKey(tamperedData, key));
    }
}