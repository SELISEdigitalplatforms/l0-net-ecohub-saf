using System;
using System.Security.Cryptography;
using System.Text;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

public static class RsaKeyHelper
{
    /// <summary>
    /// Generates an RSA key pair (public and private keys) in PEM format.
    /// </summary>
    /// <param name="keySize">The size of the RSA key in bits. Default is 2048.</param>
    /// <returns>
    /// A tuple containing the public key PEM and private key PEM as strings.
    /// <list type="bullet">
    ///   <item><description><c>publicKeyPem</c>: The RSA public key in PEM format.</description></item>
    ///   <item><description><c>privateKeyPem</c>: The RSA private key in PEM format.</description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// The returned PEM strings can be used for RSA encryption, decryption, signing, and verification operations.
    /// </remarks>
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
    /// Encrypts an AES key using the provided RSA public key in PEM format.
    /// </summary>
    /// <param name="aesKey">The AES key to encrypt as a byte array.</param>
    /// <param name="publicKey">The PEM-formatted RSA public key used for encryption.</param>
    /// <returns>A base64-encoded string of the encrypted AES key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="aesKey"/> or <paramref name="publicKey"/> is null.</exception>
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
    /// Decrypts an AES key that was encrypted with an RSA public key, using the provided RSA private key in PEM format.
    /// </summary>
    /// <param name="encryptedAesKeyBase64">The base64-encoded encrypted AES key.</param>
    /// <param name="privateKey">The PEM-formatted RSA private key used for decryption.</param>
    /// <returns>The decrypted AES key as a byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="encryptedAesKeyBase64"/> or <paramref name="privateKey"/> is null.</exception>
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

    /// <summary>
    /// Decrypts a base64-encoded string using the provided RSA private key in PEM format.
    /// </summary>
    /// <param name="content">The base64-encoded string to decrypt.</param>
    /// <param name="pemPrivateKey">The PEM-formatted RSA private key used for decryption.</param>
    /// <returns>The decrypted content as a UTF-8 string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="content"/> or <paramref name="pemPrivateKey"/> is null or empty.</exception>
    /// <exception cref="FormatException">Thrown when the base64 string is invalid.</exception>
    /// <exception cref="CryptographicException">Thrown when decryption fails.</exception>
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

    /// <summary>
    /// Imports an RSA private key from a PEM-formatted string into the provided <see cref="RSA"/> instance.
    /// </summary>
    /// <param name="rsa">The <see cref="RSA"/> instance to import the key into.</param>
    /// <param name="pem">The PEM-formatted private key string.</param>
    /// <exception cref="InvalidOperationException">Thrown when the PEM format is not supported.</exception>
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
