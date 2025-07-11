using System;
using System.Security.Cryptography;
using System.Text;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

public class EcdsaKeyHelper
{
    /// <summary>
    /// Generates a key pair (public/private) for ECDSA in PEM format.
    /// </summary>
    /// <returns>A tuple (publicKeyPem, privateKeyPem) as PEM-formatted strings.</returns>
    public static (string publicKeyPem, string privateKeyPem) GenerateKeyPair()
    {
        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        string publicKeyPem = "-----BEGIN PUBLIC KEY-----\n"
            + Convert.ToBase64String(ecdsa.ExportSubjectPublicKeyInfo())
                .Chunk(64).Select(chunk => new string(chunk) + "\n").Aggregate("", (a, b) => a + b)
            + "-----END PUBLIC KEY-----";
        string privateKeyPem = "-----BEGIN PRIVATE KEY-----\n"
            + Convert.ToBase64String(ecdsa.ExportPkcs8PrivateKey())
                .Chunk(64).Select(chunk => new string(chunk) + "\n").Aggregate("", (a, b) => a + b)
            + "-----END PRIVATE KEY-----";
        return (publicKeyPem, privateKeyPem);
    }

    public static string SignContentWithPrivateKey(string content, string pemPrivateKey)
    {
        if (string.IsNullOrEmpty(content))
            throw new ArgumentNullException(nameof(content), "Content cannot be null or empty.");
        if (string.IsNullOrEmpty(pemPrivateKey))
            throw new ArgumentNullException(nameof(pemPrivateKey), "Private key cannot be null or empty.");

        byte[] contentBytes = Encoding.UTF8.GetBytes(content);

        using var ecdsa = ECDsa.Create();
        ImportPrivateKeyFromPem(ecdsa, pemPrivateKey);

        byte[] signature = ecdsa.SignData(contentBytes, HashAlgorithmName.SHA384, DSASignatureFormat.Rfc3279DerSequence);

        return Convert.ToBase64String(signature);

    }

    static void ImportPrivateKeyFromPem(ECDsa ecdsa, string pem)
    {
        if (pem.Contains("-----BEGIN PRIVATE KEY-----"))
        {
            var base64 = KmsHelper.ExtractPemContent(pem, "PRIVATE KEY");
            byte[] keyBytes = Convert.FromBase64String(base64);
            ecdsa.ImportPkcs8PrivateKey(keyBytes, out _);
        }
        else if (pem.Contains("-----BEGIN EC PRIVATE KEY-----"))
        {
            var base64 = KmsHelper.ExtractPemContent(pem, "EC PRIVATE KEY");
            byte[] keyBytes = Convert.FromBase64String(base64);
            ecdsa.ImportECPrivateKey(keyBytes, out _);
        }
        else
        {
            throw new Exception("Unsupported EC key format.");
        }
    }

}
