using System;
using System.Security.Cryptography;
using System.Text;
using SeliseBlocks.Ecohub.Saf.Models;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

/// <summary>
/// Provides cryptographic operations for key management and data encryption/decryption.
/// </summary>
public class KmsHelper
{
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

