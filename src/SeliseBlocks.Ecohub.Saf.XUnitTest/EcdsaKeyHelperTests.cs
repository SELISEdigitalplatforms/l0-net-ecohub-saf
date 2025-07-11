using System;
using System.Security.Cryptography;
using SeliseBlocks.Ecohub.Saf.Helpers;
using Xunit;

namespace SeliseBlocks.Ecohub.Saf.XUnitTest;

public class EcdsaKeyHelperTests
{
    [Fact]
    public void GenerateKeyPair_ReturnsValidPem()
    {
        // Act
        var (publicKeyPem, privateKeyPem) = EcdsaKeyHelper.GenerateKeyPair();

        // Assert
        Assert.NotNull(publicKeyPem);
        Assert.NotNull(privateKeyPem);
        Assert.StartsWith("-----BEGIN PUBLIC KEY-----", publicKeyPem.Trim());
        Assert.StartsWith("-----BEGIN PRIVATE KEY-----", privateKeyPem.Trim());
        Assert.EndsWith("-----END PUBLIC KEY-----", publicKeyPem.Trim());
        Assert.EndsWith("-----END PRIVATE KEY-----", privateKeyPem.Trim());

        // Try to import keys to verify they are valid
        using var ecdsaPub = ECDsa.Create();
        using var ecdsaPriv = ECDsa.Create();
        Exception exPub = Record.Exception(() => ecdsaPub.ImportFromPem(publicKeyPem));
        Exception exPriv = Record.Exception(() => ecdsaPriv.ImportFromPem(privateKeyPem));
        Assert.Null(exPub);
        Assert.Null(exPriv);
    }

    [Fact]
    public void SignContentWithPrivateKey_ReturnsValidSignature()
    {
        // Arrange
        var (publicKeyPem, privateKeyPem) = EcdsaKeyHelper.GenerateKeyPair();
        string content = "Hello, ECDSA!";

        // Act
        string signatureBase64 = EcdsaKeyHelper.SignContentWithPrivateKey(content, privateKeyPem);

        // Assert
        Assert.False(string.IsNullOrEmpty(signatureBase64));
        byte[] signature = Convert.FromBase64String(signatureBase64);
        Assert.NotNull(signature);
        Assert.True(signature.Length > 0);

        // Verify signature
        using var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(publicKeyPem);
        byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(content);
        bool isValid = ecdsa.VerifyData(contentBytes, signature, HashAlgorithmName.SHA384, DSASignatureFormat.Rfc3279DerSequence);
        Assert.True(isValid);
    }

    [Fact]
    public void SignContentWithPrivateKey_ThrowsOnNullArguments()
    {
        var (_, privateKeyPem) = EcdsaKeyHelper.GenerateKeyPair();
        Assert.Throws<ArgumentNullException>(() => EcdsaKeyHelper.SignContentWithPrivateKey(null, privateKeyPem));
        Assert.Throws<ArgumentNullException>(() => EcdsaKeyHelper.SignContentWithPrivateKey("test", null));
        Assert.Throws<ArgumentNullException>(() => EcdsaKeyHelper.SignContentWithPrivateKey(string.Empty, privateKeyPem));
        Assert.Throws<ArgumentNullException>(() => EcdsaKeyHelper.SignContentWithPrivateKey("test", string.Empty));
    }
}
