using System;
using System.IO.Compression;
using System.Text;

namespace SeliseBlocks.Ecohub.Saf;

/// <summary>
/// Provides static methods for compressing and decompressing data using GZIP compression.
/// </summary>
public static class GzipCompressor
{
    /// <summary>
    /// Compresses a byte array using GZIP compression.
    /// </summary>
    /// <param name="data">The byte array to compress.</param>
    /// <returns>A compressed byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when compression fails.</exception>
    public static byte[] CompressBytes(byte[] data)
    {
        using var compressedStream = new MemoryStream();
        using (var gzip = new GZipStream(compressedStream, CompressionLevel.Optimal, leaveOpen: true))
        {
            gzip.Write(data, 0, data.Length);
        }
        return compressedStream.ToArray();
    }

    /// <summary>
    /// Decompresses a GZIP compressed byte array to a string using UTF-8 encoding.
    /// </summary>
    /// <param name="compressedData">The compressed byte array to decompress.</param>
    /// <returns>The decompressed string in UTF-8 encoding.</returns>
    /// <exception cref="ArgumentNullException">Thrown when compressedData is null.</exception>
    /// <exception cref="InvalidDataException">Thrown when the compressed data is invalid or corrupted.</exception>
    public static string DecompressToString(byte[] compressedData)
    {
        using var compressedStream = new MemoryStream(compressedData);
        using var gzip = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var reader = new StreamReader(gzip, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Decompresses a GZIP compressed byte array back to its original byte array form.
    /// </summary>
    /// <param name="compressedData">The compressed byte array to decompress.</param>
    /// <returns>The decompressed byte array.</returns>
    /// <exception cref="ArgumentNullException">Thrown when compressedData is null.</exception>
    /// <exception cref="InvalidDataException">Thrown when the compressed data is invalid or corrupted.</exception>
    public static byte[] DecompressToBytes(byte[] compressedData)
    {
        using var compressedStream = new MemoryStream(compressedData);
        using var gzip = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var decompressedStream = new MemoryStream();
        gzip.CopyTo(decompressedStream);
        return decompressedStream.ToArray();
    }
}
