using System;
using System.Security.Cryptography;
using System.Text;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

public class EcdsaKeyHelper
{

    /// <summary>
    /// Generates an ECDSA key pair (public and private keys) in PEM format using the NIST P-256 curve.
    /// </summary>
    /// <returns>
    /// A tuple containing the public key PEM and private key PEM as strings.
    /// <list type="bullet">
    ///   <item><description><c>publicKeyPem</c>: The ECDSA public key in PEM format.</description></item>
    ///   <item><description><c>privateKeyPem</c>: The ECDSA private key in PEM format.</description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// The returned PEM strings can be used for ECDSA signing and verification operations.
    /// </remarks>
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

    /// <summary>
    /// Signs the specified content using the provided ECDSA private key in PEM format.
    /// </summary>
    /// <param name="content">The string content to sign.</param>
    /// <param name="pemPrivateKey">The ECDSA private key in PEM format.</param>
    /// <returns>The base64-encoded signature of the content.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="content"/> or <paramref name="pemPrivateKey"/> is null or empty.</exception>
    /// <remarks>
    /// Uses SHA-384 as the hash algorithm and DER sequence format for the signature.
    /// </remarks>
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
