using System;
using System.Security.Cryptography;

namespace SeliseBlocks.Ecohub.Saf;

internal class SafEventService : ISafEventService
{
    private readonly IHttpRequestGateway _httpRequestGateway;

    public SafEventService(IHttpRequestGateway httpRequestGateway)
    {
        _httpRequestGateway = httpRequestGateway;
    }

    public async Task<SafSendOfferNlpiEventResponse> SendOfferNlpiEventAsync(SafSendOfferNlpiEventRequest request)
    {
        var header = new Dictionary<string, string>
        {
            { "schemaVersionId", request.SchemaVersionId },
            { "keySchemaVersionId", request.KeySchemaVersionId }
        };

        var payload = request.EventPayload.MapToSafOfferNlpiEncryptedEvent();
        payload.Data = GetCompressAndEncryptEventData(request.EventPayload.Data);
        var response = await _httpRequestGateway.PostAsync<SafOfferNlpiEncryptedEvent, SafSendOfferNlpiEventResponse>(
            SafDriverConstant.SendOfferNlpiEventEndpoint,
            payload,
            header,
            request.BearerToken);

        return response;
    }
    public async Task<IEnumerable<SafOfferNlpiEvent>> ReceiveOfferNlpiEventAsync(SafReceiveOfferNlpiEventRequest request)
    {
        var endpoint = SafDriverConstant.ReceiveOfferNlpiEventEndpoint.Replace("{ecohubId}", request.EcohubId);
        var header = new Dictionary<string, string>
        {
            { "auto.offset.reset", request.AutoOffsetReset }
        };
        var eventResponses = await _httpRequestGateway.GetAsync<IEnumerable<SafReceiveOfferNlpiEventResponse>>(
            endpoint,
            header,
            request.BearerToken);

        var events = new List<SafOfferNlpiEvent>();
        foreach (var eventItem in eventResponses)
        {
            var eventResponse = eventItem.Value.MapToSafOfferNlpiEvent();

            var data = GetDecompressAndDecryptEventData(eventItem.Value.Data, request.PrivateKey);
            eventResponse.Data = data;
            events.Add(eventResponse);
        }

        return events; ;
    }

    private SafEncryptedData GetCompressAndEncryptEventData(SafData data)
    {
        var compressData = GzipCompressor.CompressBytes(data.Payload);
        var aesKey = GenerateAesKey();

        var encryptedData = AesGcmEncryptor.Encrypt(compressData, aesKey);
        var encryptedAesKey = EncryptAesKey(aesKey, data.PublicKey);
        return new SafEncryptedData
        {
            Payload = encryptedData,
            EncryptionKey = encryptedAesKey,
            PublicKeyVersion = data.PublicKeyVersion,
            Links = data.Links,
            Message = data.Message
        };
    }

    private SafData GetDecompressAndDecryptEventData(SafEncryptedData data, string privateKey)
    {
        var aesKey = DecryptAesKey(data.EncryptionKey, privateKey);

        var decryptedData = AesGcmEncryptor.Decrypt(data.Payload, aesKey);
        var unZippedData = GzipCompressor.DecompressToBytes(decryptedData);

        return new SafData
        {
            Payload = unZippedData,
            PublicKeyVersion = data.PublicKeyVersion,
            Links = data.Links,
            Message = data.Message
        };
    }

    private byte[] GenerateAesKey()
    {
        // 1. Generate AES key
        using Aes aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();

        byte[] aesKey = aes.Key;
        return aesKey;
    }
    private string EncryptAesKey(byte[] aesKey, string publicKey)
    {
        // Encrypt AES key with the public key
        using RSA rsa = RSA.Create();

        // Import the public key directly using RSA class
        rsa.ImportFromPem(publicKey.ToCharArray());

        byte[] encryptedAesKey = rsa.Encrypt(aesKey, RSAEncryptionPadding.Pkcs1);
        return Convert.ToBase64String(encryptedAesKey);
    }
    private byte[] DecryptAesKey(string encryptedAesKeyBase64, string privateKey)
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
}
