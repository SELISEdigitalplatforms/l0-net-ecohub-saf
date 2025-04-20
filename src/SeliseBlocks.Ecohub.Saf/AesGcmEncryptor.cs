using System;
using System.Security.Cryptography;

namespace SeliseBlocks.Ecohub.Saf;

public class AesGcmEncryptor
{
    public static string Encrypt(byte[] payload, byte[] key)
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

    public static byte[] Decrypt(string base64Data, byte[] key)
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
}
