using System;
using System.Security.Cryptography;
using SeliseBlocks.Ecohub.Saf.Helpers;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class RsaKeyHelperTests
{
    [Fact]
    public void GenerateKeyPair_ReturnsValidPem()
    {
        var (publicKey, privateKey) = RsaKeyHelper.GenerateKeyPair();
        Assert.StartsWith("-----BEGIN PUBLIC KEY-----", publicKey.Trim());
        Assert.EndsWith("-----END PUBLIC KEY-----", publicKey.Trim());
        Assert.StartsWith("-----BEGIN PRIVATE KEY-----", privateKey.Trim());
        Assert.EndsWith("-----END PRIVATE KEY-----", privateKey.Trim());
        // Try to import keys
        using var rsaPub = RSA.Create();
        using var rsaPriv = RSA.Create();
        Exception exPub = Record.Exception(() => rsaPub.ImportFromPem(publicKey));
        Exception exPriv = Record.Exception(() => rsaPriv.ImportFromPem(privateKey));
        Assert.Null(exPub);
        Assert.Null(exPriv);
    }

    [Fact]
    public void EncryptAesKeyWithPublicKey_And_DecryptAesKeyWithPrivateKey_Works()
    {
        var (publicKey, privateKey) = RsaKeyHelper.GenerateKeyPair();
        var aesKey = new byte[32];
        new Random().NextBytes(aesKey);
        var encrypted = RsaKeyHelper.EncryptAesKeyWithPublicKey(aesKey, publicKey);
        var decrypted = RsaKeyHelper.DecryptAesKeyWithPrivateKey(encrypted, privateKey);
        Assert.Equal(aesKey, decrypted);
    }

    [Fact]
    public void DecryptContentWithPrivateKey_ReturnsOriginalContent()
    {
        var (publicKey, privateKey) = RsaKeyHelper.GenerateKeyPair();
        var content = "Hello RSA!";
        using var rsa = RSA.Create();
        rsa.ImportFromPem(publicKey);
        var encrypted = Convert.ToBase64String(rsa.Encrypt(System.Text.Encoding.UTF8.GetBytes(content), RSAEncryptionPadding.OaepSHA256));
        var decrypted = RsaKeyHelper.DecryptContentWithPrivateKey(encrypted, privateKey);
        Assert.Equal(content, decrypted);
    }

    [Fact]
    public void ImportPrivateKeyFromPem_ThrowsOnInvalidPem()
    {
        using var rsa = RSA.Create();
        Assert.Throws<InvalidOperationException>(() => RsaKeyHelper.ImportPrivateKeyFromPem(rsa, "INVALID_PEM"));
    }
}
