using System;
using System.Security.Cryptography;
using System.Text;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

public static class RsaKeyHelper
{
    /// <summary>
    /// Generates a key pair (public/private) for RSA in PEM format.
    /// </summary>
    /// <param name="keySize">Key size for RSA (default 2048)</param>
    /// <returns>A tuple (publicKeyPem, privateKeyPem) as PEM-formatted strings.</returns>
    public static (string publicKeyPem, string privateKeyPem) GenerateKeyPair(int keySize = 2048)
    {
        using var rsa = RSA.Create(keySize);
        string publicKeyPem = "-----BEGIN PUBLIC KEY-----\n"
            + Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo())
                .Chunk(64).Select(chunk => new string(chunk) + "\n").Aggregate("", (a, b) => a + b)
            + "-----END PUBLIC KEY-----";
        string privateKeyPem = "-----BEGIN PRIVATE KEY-----\n"
            + Convert.ToBase64String(rsa.ExportPkcs8PrivateKey())
                .Chunk(64).Select(chunk => new string(chunk) + "\n").Aggregate("", (a, b) => a + b)
            + "-----END PRIVATE KEY-----";
        return (publicKeyPem, privateKeyPem);
    }

    /// <summary>
    /// Encrypts an AES key using RSA public key encryption.
    /// </summary>
    /// <param name="aesKey">The AES key to encrypt.</param>
    /// <param name="publicKey">The PEM-formatted RSA public key used for encryption.</param>
    /// <returns>Base64-encoded string of the encrypted AES key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when aesKey or publicKey is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the public key format is invalid.</exception>
    /// <exception cref="CryptographicException">Thrown when encryption fails.</exception>
    public static string EncryptAesKeyWithPublicKey(byte[] aesKey, string publicKey)
    {
        // Encrypt AES key with the public key
        using RSA rsa = RSA.Create();

        // Import the public key directly using RSA class
        rsa.ImportFromPem(publicKey.ToCharArray());

        byte[] encryptedAesKey = rsa.Encrypt(aesKey, RSAEncryptionPadding.Pkcs1);
        return Convert.ToBase64String(encryptedAesKey);
    }

    /// <summary>
    /// Decrypts an encrypted AES key using RSA private key decryption.
    /// </summary>
    /// <param name="encryptedAesKeyBase64">Base64-encoded encrypted AES key.</param>
    /// <param name="privateKey">The PEM-formatted RSA private key used for decryption.</param>
    /// <returns>The decrypted AES key as a byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when encryptedAesKeyBase64 or privateKey is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the private key format is invalid.</exception>
    /// <exception cref="FormatException">Thrown when the base64 string is invalid.</exception>
    /// <exception cref="CryptographicException">Thrown when decryption fails.</exception>
    public static byte[] DecryptAesKeyWithPrivateKey(string encryptedAesKeyBase64, string privateKey)
    {
        byte[] aesKey = Convert.FromBase64String(encryptedAesKeyBase64);
        // Set up RSA for decryption
        RSA rsaPrivate = RSA.Create();

        // Import the private key
        rsaPrivate.ImportFromPem(privateKey);

        // Decrypt the AES key
        byte[] decryptedAesKey = rsaPrivate.Decrypt(aesKey, RSAEncryptionPadding.Pkcs1);

        return decryptedAesKey;
    }

    public static string DecryptContentWithPrivateKey(string content, string pemPrivateKey)
    {
        if (string.IsNullOrEmpty(content))
            throw new ArgumentNullException(nameof(content), "Content cannot be null or empty.");

        if (string.IsNullOrEmpty(pemPrivateKey))
            throw new ArgumentNullException(nameof(pemPrivateKey), "Private key cannot be null or empty.");

        byte[] contentBytes = Convert.FromBase64String(content);
        using RSA rsa = RSA.Create();
        ImportPrivateKeyFromPem(rsa, pemPrivateKey);

        byte[] decrypted = rsa.Decrypt(contentBytes, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decrypted);
    }

    public static void ImportPrivateKeyFromPem(RSA rsa, string pem)
    {
        byte[] keyBytes;
        if (pem.Contains("-----BEGIN PRIVATE KEY-----"))
        {
            keyBytes = Convert.FromBase64String(KmsHelper.ExtractPemContent(pem, "PRIVATE KEY"));
            rsa.ImportPkcs8PrivateKey(keyBytes, out _);
        }
        else if (pem.Contains("-----BEGIN RSA PRIVATE KEY-----"))
        {
            keyBytes = Convert.FromBase64String(KmsHelper.ExtractPemContent(pem, "RSA PRIVATE KEY"));
            rsa.ImportRSAPrivateKey(keyBytes, out _);
        }
        else
        {
            throw new InvalidOperationException("Unsupported PEM format");
        }
    }

}
