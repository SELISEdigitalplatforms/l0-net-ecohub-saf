using SeliseBlocks.Ecohub.Saf.Helpers;

namespace SeliseBlocks.Ecohub.Saf.Services.Kafka;

public static class SafCryptoUtils
{
    public static SafEncryptedData CompressAndEncrypt(SafData data)
    {
        var compressData = GzipCompressor.CompressBytes(data.Payload);
        var aesKey = KmsHelper.GenerateAesKey();
        var encryptedData = KmsHelper.EncryptWithAesKey(compressData, aesKey);
        var encryptedAesKey = KmsHelper.EncryptAesKeyWithPublicKey(aesKey, data.PublicKey);

        return new SafEncryptedData
        {
            Payload = encryptedData,
            EncryptionKey = encryptedAesKey,
            PublicKeyVersion = data.PublicKeyVersion,
            Links = data.Links,
            Message = data.Message
        };
    }
    public static SafEncryptedKafkaData CompressAndEncryptForKafka(SafData data)
    {
        var compressData = GzipCompressor.CompressBytes(data.Payload);
        var aesKey = KmsHelper.GenerateAesKey();
        var encryptedData = KmsHelper.EncryptWithAesKey(compressData, aesKey);
        var encryptedAesKey = KmsHelper.EncryptAesKeyWithPublicKey(aesKey, data.PublicKey);

        return new SafEncryptedKafkaData
        {
            payload = encryptedData,
            encryptionKey = encryptedAesKey,
            publicKeyVersion = data.PublicKeyVersion,
            links = data.Links,
            message = data.Message
        };
    }

    public static SafData DecryptAndDecompress(SafEncryptedData encryptedData, string privateKey)
    {
        var aesKey = KmsHelper.DecryptAesKeyWithPrivateKey(encryptedData.EncryptionKey, privateKey);
        var decryptedBytes = KmsHelper.DecryptWithAesKey(encryptedData.Payload, aesKey);
        var uncompressed = GzipCompressor.DecompressToBytes(decryptedBytes);

        return new SafData
        {
            Payload = uncompressed,
            PublicKeyVersion = encryptedData.PublicKeyVersion,
            Links = encryptedData.Links,
            Message = encryptedData.Message
        };
    }
}

