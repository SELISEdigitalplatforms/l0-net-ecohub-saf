using System;
using System.IO.Compression;
using System.Text;

namespace SeliseBlocks.Ecohub.Saf;

internal static class GzipCompressor
{
    public static byte[] CompressBytes(byte[] data)
    {
        using var compressedStream = new MemoryStream();
        using (var gzip = new GZipStream(compressedStream, CompressionLevel.Optimal, leaveOpen: true))
        {
            gzip.Write(data, 0, data.Length);
        }
        return compressedStream.ToArray();
    }
    public static string DecompressToString(byte[] compressedData)
    {
        using var compressedStream = new MemoryStream(compressedData);
        using var gzip = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var reader = new StreamReader(gzip, Encoding.UTF8);
        return reader.ReadToEnd();
    }
    public static byte[] DecompressToBytes(byte[] compressedData)
    {
        using var compressedStream = new MemoryStream(compressedData);
        using var gzip = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var decompressedStream = new MemoryStream();
        gzip.CopyTo(decompressedStream);
        return decompressedStream.ToArray();
    }
}
