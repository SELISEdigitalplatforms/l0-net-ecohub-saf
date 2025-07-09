using System;
using System.Security.Cryptography;
using System.Text;
using SeliseBlocks.Ecohub.Saf.Models;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

/// <summary>
/// Provides cryptographic operations for key management and data encryption/decryption.
/// Supports AES key generation, RSA encryption/decryption, and AES-GCM encryption/decryption.
/// </summary>
public class KmsHelper
{

    /// <summary>
    /// Generates a new 256-bit AES key.
    /// </summary>
    /// <returns>A byte array containing the generated 256-bit AES key.</returns>
    public static byte[] GenerateAesKey()
    {
        // 1. Generate AES key
        using Aes aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();

        byte[] aesKey = aes.Key;
        return aesKey;
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

    /// <summary>
    /// Encrypts data using AES-GCM encryption with the specified key.
    /// </summary>
    /// <param name="payload">The data to encrypt.</param>
    /// <param name="key">The 256-bit (32-byte) AES key used for encryption.</param>
    /// <returns>Base64-encoded string containing the IV, ciphertext, and authentication tag.</returns>
    /// <exception cref="ArgumentNullException">Thrown when payload or key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the key length is not 32 bytes (256 bits).</exception>
    /// <exception cref="CryptographicException">Thrown when encryption fails.</exception>
    /// <remarks>
    /// The output format is: [12-byte IV][N-byte ciphertext][16-byte authentication tag]
    /// </remarks>
    public static string EncryptWithAesKey(byte[] payload, byte[] key)
    {
        try
        {
            if (key.Length != 32)
                throw new ArgumentException("Key must be 256 bits (32 bytes)");

            // Generate a random 96-bit (12-byte) IV
            byte[] iv = new byte[12];
            RandomNumberGenerator.Fill(iv);

            // Prepare output buffers
            byte[] ciphertext = new byte[payload.Length];
            byte[] tag = new byte[16]; // 128-bit tag

            // Encrypt using AES-GCM
            using var aesGcm = new AesGcm(key, 16); // Specify the tag size (16 bytes for 128-bit tag)
            aesGcm.Encrypt(iv, payload, ciphertext, tag);

            // Combine IV + ciphertext + tag
            byte[] result = new byte[iv.Length + ciphertext.Length + tag.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(ciphertext, 0, result, iv.Length, ciphertext.Length);
            Buffer.BlockCopy(tag, 0, result, iv.Length + ciphertext.Length, tag.Length);

            // Return Base64-encoded result
            return Convert.ToBase64String(result);
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException("Encryption failed.", ex);
        }
        catch (ArgumentNullException ex)
        {
            throw new ArgumentNullException("Input data or key cannot be null.", ex);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Key must be 256 bits (32 bytes).", ex);
        }
        catch (FormatException ex)
        {
            throw new FormatException("Invalid base64 string format.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred during encryption.", ex);
        }
    }


    /// <summary>
    /// Decrypts data using AES-GCM decryption with the specified key.
    /// </summary>
    /// <param name="base64Data">Base64-encoded string containing the IV, ciphertext, and authentication tag.</param>
    /// <param name="key">The 256-bit (32-byte) AES key used for decryption.</param>
    /// <returns>The decrypted data as a byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when base64Data or key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the key length is not 32 bytes (256 bits).</exception>
    /// <exception cref="FormatException">Thrown when the base64 string is invalid.</exception>
    /// <exception cref="CryptographicException">Thrown when decryption fails or authentication fails.</exception>
    /// <remarks>
    /// Expects input format: [12-byte IV][N-byte ciphertext][16-byte authentication tag]
    /// </remarks>
    public static byte[] DecryptWithAesKey(string base64Data, byte[] key)
    {
        try
        {
            byte[] input = Convert.FromBase64String(base64Data);

            byte[] iv = new byte[12];
            Buffer.BlockCopy(input, 0, iv, 0, iv.Length);

            int tagLength = 16;
            int ciphertextLength = input.Length - iv.Length - tagLength;

            byte[] ciphertext = new byte[ciphertextLength];
            byte[] tag = new byte[tagLength];

            Buffer.BlockCopy(input, iv.Length, ciphertext, 0, ciphertext.Length);
            Buffer.BlockCopy(input, iv.Length + ciphertext.Length, tag, 0, tag.Length);

            using var aesGcm = new AesGcm(key, 16); // Specify the tag size (16 bytes for 128-bit tag)
            byte[] decrypted = new byte[ciphertext.Length];
            aesGcm.Decrypt(iv, ciphertext, tag, decrypted);

            return decrypted;
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException("Decryption failed or authentication failed.", ex);
        }
        catch (FormatException ex)
        {
            throw new FormatException("Invalid base64 string format.", ex);
        }
        catch (ArgumentNullException ex)
        {
            throw new ArgumentNullException("Input data or key cannot be null.", ex);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Key must be 256 bits (32 bytes).", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred during decryption.", ex);
        }
    }


    /// <summary>
    /// Generates an RSA key in PEM format for the specified key type and size.
    /// </summary>
    /// <param name="keyType">
    /// The type of RSA key to generate: 
    /// <see cref="RsaKeyType.Public"/> for a public key.
    /// <see cref="RsaKeyType.Private"/> for a private key.
    /// <see cref="RsaKeyType.Both"/> to get both keys.
    /// </param>
    /// <param name="keySize">
    /// The size of the RSA key in bits. Default is 2048.
    /// </param>
    /// <returns>
    /// A dictionary containing the generated key(s) as PEM-formatted strings, keyed by <see cref="RsaKeyType"/>.
    /// </returns>
    /// <remarks>
    /// The returned dictionary will contain an entry for each requested key type. Each value is a PEM-formatted string.
    /// </remarks>
    public static Dictionary<RsaKeyType, string> GenerateRsaKey(RsaKeyType keyType, int keySize = 2048)
    {
        var keys = new Dictionary<RsaKeyType, string>();
        try
        {
            using var rsa = RSA.Create(keySize);
            if (keyType == RsaKeyType.Public || keyType == RsaKeyType.Both)
            {
                keys[RsaKeyType.Public] = string.Concat("-----BEGIN PUBLIC KEY-----\n",
                                           Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo())
                                                  .Chunk(64)
                                                  .Select(chunk => new string(chunk) + "\n"),
                                           "-----END PUBLIC KEY-----");
            }
            if (keyType == RsaKeyType.Private || keyType == RsaKeyType.Both)
            {
                keys[RsaKeyType.Private] = string.Concat("-----BEGIN PRIVATE KEY-----\n",
                                           Convert.ToBase64String(rsa.ExportPkcs8PrivateKey())
                                                  .Chunk(64)
                                                  .Select(chunk => new string(chunk) + "\n"),
                                           "-----END PRIVATE KEY-----");
            }

        }
        catch (Exception)
        {
        }
        return keys;
    }

    public static string GetSignatureOfVerificationContent(string verificationContent, string base64PrivateKey)
    {
        if (string.IsNullOrEmpty(verificationContent))
            throw new ArgumentNullException(nameof(verificationContent), "Verification content cannot be null or empty.");

        if (string.IsNullOrEmpty(base64PrivateKey))
            throw new ArgumentNullException(nameof(base64PrivateKey), "Private key cannot be null or empty.");


        //string base64PrivateKey = "MIGHAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBG0wawIBAQQg9PfSPzYfM1r6X..."; // example
        byte[] contentBytes = System.Text.Encoding.UTF8.GetBytes(verificationContent);

        // Step 1: Import EC private key (Base64 PKCS#8 DER format)
        byte[] privateKeyBytes = Convert.FromBase64String(base64PrivateKey);
        using var ecdsa = ECDsa.Create();
        ecdsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

        // Step 2: Sign content with SHA384
        byte[] signature = ecdsa.SignData(contentBytes, HashAlgorithmName.SHA384);

        // Step 3: Convert signature to Base64
        return Convert.ToBase64String(signature);

    }
    public static string GetDecryptedContent(string content, string pemPrivateKey)
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
            keyBytes = Convert.FromBase64String(ExtractPemContent(pem, "PRIVATE KEY"));
            rsa.ImportPkcs8PrivateKey(keyBytes, out _);
        }
        else if (pem.Contains("-----BEGIN RSA PRIVATE KEY-----"))
        {
            keyBytes = Convert.FromBase64String(ExtractPemContent(pem, "RSA PRIVATE KEY"));
            rsa.ImportRSAPrivateKey(keyBytes, out _);
        }
        else
        {
            throw new InvalidOperationException("Unsupported PEM format");
        }
    }
    public static string ExtractPemContent(string pem, string label)
    {
        string header = $"-----BEGIN {label}-----";
        string footer = $"-----END {label}-----";
        int start = pem.IndexOf(header) + header.Length;
        int end = pem.IndexOf(footer, start);
        string base64 = pem[start..end].Replace("\r", "").Replace("\n", "").Trim();
        return base64;
    }



}

