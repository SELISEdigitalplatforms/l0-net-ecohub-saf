using System.Security.Cryptography;
using System.Text;
using SeliseBlocks.Ecohub.Saf.Helpers;
using SeliseBlocks.Ecohub.Saf.Models;

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
        var key = AesKeyHelper.GenerateKey();
        Assert.NotNull(key);
        Assert.Equal(32, key.Length);
    }

    [Fact]
    public void EncryptAesKeyWithPublicKey_ShouldEncryptKey()
    {
        var aesKey = AesKeyHelper.GenerateKey();
        var encryptedKey = RsaKeyHelper.EncryptAesKeyWithPublicKey(aesKey, _testPublicKey);
        Assert.NotNull(encryptedKey);
        Assert.True(Convert.TryFromBase64String(encryptedKey, new byte[encryptedKey.Length], out _));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-pem")]
    public void EncryptAesKeyWithPublicKey_ShouldThrowException_WhenPublicKeyIsInvalid(string invalidKey)
    {
        var aesKey = AesKeyHelper.GenerateKey();
        Assert.ThrowsAny<Exception>(() => RsaKeyHelper.EncryptAesKeyWithPublicKey(aesKey, invalidKey));
    }

    [Fact]
    public void EncryptAesKeyWithPublicKey_ShouldThrowException_WhenAesKeyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            RsaKeyHelper.EncryptAesKeyWithPublicKey(null, _testPublicKey));
    }

    [Fact]
    public void DecryptAesKeyWithPrivateKey_ShouldDecryptKey()
    {
        var originalKey = AesKeyHelper.GenerateKey();
        var encryptedKey = RsaKeyHelper.EncryptAesKeyWithPublicKey(originalKey, _testPublicKey);
        var decryptedKey = RsaKeyHelper.DecryptAesKeyWithPrivateKey(encryptedKey, _testPrivateKey);
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
            RsaKeyHelper.DecryptAesKeyWithPrivateKey(encryptedKey, invalidKey));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-base64")]
    public void DecryptAesKeyWithPrivateKey_ShouldThrowException_WhenEncryptedKeyIsInvalid(string invalidKey)
    {
        Assert.ThrowsAny<Exception>(() =>
            RsaKeyHelper.DecryptAesKeyWithPrivateKey(invalidKey, _testPrivateKey));
    }

    [Fact]
    public void EncryptWithAesKey_ShouldEncryptData()
    {
        var key = AesKeyHelper.GenerateKey();
        var data = Encoding.UTF8.GetBytes("Test data");
        var encryptedData = AesKeyHelper.Encrypt(data, key);
        Assert.NotNull(encryptedData);
        Assert.True(Convert.TryFromBase64String(encryptedData, new byte[encryptedData.Length], out _));
    }

    [Fact]
    public void EncryptWithAesKey_ShouldThrowException_WhenPayloadIsNull()
    {
        var key = AesKeyHelper.GenerateKey();
        Assert.Throws<Exception>(() => AesKeyHelper.Encrypt(null, key));
    }

    [Fact]
    public void EncryptWithAesKey_ShouldThrowException_WhenKeyIsNull()
    {
        var data = Encoding.UTF8.GetBytes("Test data");
        Assert.Throws<Exception>(() => AesKeyHelper.Encrypt(data, null));
    }

    [Fact]
    public void EncryptWithAesKey_ShouldThrowException_WhenKeyLengthIsInvalid()
    {
        var data = Encoding.UTF8.GetBytes("Test data");
        var invalidKey = new byte[16];
        Assert.Throws<ArgumentException>(() => AesKeyHelper.Encrypt(data, invalidKey));
    }

    [Fact]
    public void DecryptWithAesKey_ShouldDecryptData()
    {
        var key = AesKeyHelper.GenerateKey();
        var originalData = Encoding.UTF8.GetBytes("Test data");
        var encryptedData = AesKeyHelper.Encrypt(originalData, key);
        var decryptedData = AesKeyHelper.Decrypt(encryptedData, key);
        Assert.Equal(originalData, decryptedData);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("invalid-base64")]
    public void DecryptWithAesKey_ShouldThrowException_WhenBase64DataIsInvalid(string invalidData)
    {
        var key = AesKeyHelper.GenerateKey();
        Assert.ThrowsAny<Exception>(() => AesKeyHelper.Decrypt(invalidData, key));
    }

    [Fact]
    public void DecryptWithAesKey_ShouldThrowException_WhenKeyIsNull()
    {
        var encryptedData = Convert.ToBase64String(new byte[] { 1, 2, 3 });
        Assert.Throws<ArgumentException>(() => AesKeyHelper.Decrypt(encryptedData, null));
    }

    [Fact]
    public void DecryptWithAesKey_ShouldThrowException_WhenKeyLengthIsInvalid()
    {
        var encryptedData = Convert.ToBase64String(new byte[] { 1, 2, 3 });
        var invalidKey = new byte[16];
        Assert.Throws<ArgumentException>(() => AesKeyHelper.Decrypt(encryptedData, invalidKey));
    }

    [Fact]
    public void DecryptWithAesKey_ShouldThrowException_WhenAuthenticationFails()
    {
        var key = AesKeyHelper.GenerateKey();
        var data = Encoding.UTF8.GetBytes("Test data");
        var encryptedData = AesKeyHelper.Encrypt(data, key);
        var tamperedData = string.Concat(encryptedData.AsSpan(0, encryptedData.Length - 4), "AAAA");

        Assert.Throws<CryptographicException>(() =>
            AesKeyHelper.Decrypt(tamperedData, key));
    }

    [Fact]
    public void GenerateKeyPair_ReturnsValidPem()
    {
        using var rsa = RSA.Create(2048);
        var privateKey = PemEncoding.Write("PRIVATE KEY", rsa.ExportPkcs8PrivateKey());
        var publicKey = PemEncoding.Write("PUBLIC KEY", rsa.ExportSubjectPublicKeyInfo());

        // Try to import both
        using var rsaPriv = RSA.Create();
        using var rsaPub = RSA.Create();
        rsaPriv.ImportFromPem(privateKey);
        rsaPub.ImportFromPem(publicKey);
    }

}