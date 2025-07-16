
namespace SeliseBlocks.Ecohub.Saf.Helpers;

public static class SafEventDataResolver
{
    public static SafEncryptedData CompressEncryptAndSignPayload(SafData data)
    {
        var eventData = CompressAndEncrypt(data);
        eventData.PayloadSignature = EcdsaKeyHelper.SignContentWithPrivateKey(eventData.Payload, data.EcPrivateKey);
        return eventData;
    }

    public static SafEncryptedData CompressAndEncrypt(SafData data)
    {
        var (encryptedData, encryptedAesKey) = GetEncryptedDataAndKey(data);

        return new SafEncryptedData
        {
            Payload = encryptedData,
            EncryptionKey = encryptedAesKey,
            PublicKeyVersion = data.PublicKeyVersion,
            SignatureKeyVersion = data.SignatureKeyVersion,
            Links = data.Links
            // Message = data.Message
        };
    }

    public static SafEncryptedKafkaData CompressAndEncryptForKafka(SafData data)
    {
        var (encryptedData, encryptedAesKey) = GetEncryptedDataAndKey(data);

        return new SafEncryptedKafkaData
        {
            payload = encryptedData,
            encryptionKey = encryptedAesKey,
            publicKeyVersion = data.PublicKeyVersion
        };
    }

    private static (string encryptedData, string encryptedAesKey) GetEncryptedDataAndKey(SafData data)
    {
        var compressData = GzipCompressor.CompressBytes(data.Payload);
        var aesKey = AesKeyHelper.GenerateKey();
        var encryptedData = AesKeyHelper.Encrypt(compressData, aesKey);
        var encryptedAesKey = RsaKeyHelper.EncryptAesKeyWithPublicKey(aesKey, data.PublicKey);

        return (encryptedData, encryptedAesKey);
    }

    public static SafData DecryptAndDecompress(SafEncryptedData encryptedData, string privateKey)
    {
        var aesKey = RsaKeyHelper.DecryptAesKeyWithPrivateKey(encryptedData.EncryptionKey, privateKey);
        var decryptedBytes = AesKeyHelper.Decrypt(encryptedData.Payload, aesKey);
        var uncompressed = GzipCompressor.DecompressToBytes(decryptedBytes);

        return new SafData
        {
            Payload = uncompressed,
            PublicKeyVersion = encryptedData.PublicKeyVersion,
            Links = encryptedData.Links
            // Message = encryptedData.Message
        };
    }
}
