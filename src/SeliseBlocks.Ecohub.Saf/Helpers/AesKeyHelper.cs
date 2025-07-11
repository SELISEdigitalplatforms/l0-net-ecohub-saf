using System;
using System.Security.Cryptography;
using System.Text;
using SeliseBlocks.Ecohub.Saf.Models;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

/// <summary>
/// Provides cryptographic operations for key management and data encryption/decryption.
/// </summary>
public class AesKeyHelper
{

    /// <summary>
    /// Generates a new 256-bit AES key.
    /// </summary>
    /// <returns>A byte array containing the generated 256-bit AES key.</returns>
    public static byte[] GenerateKey()
    {
        // 1. Generate AES key
        using Aes aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();

        byte[] aesKey = aes.Key;
        return aesKey;
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
    public static string Encrypt(byte[] payload, byte[] key)
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
    public static byte[] Decrypt(string base64Data, byte[] key)
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


}

