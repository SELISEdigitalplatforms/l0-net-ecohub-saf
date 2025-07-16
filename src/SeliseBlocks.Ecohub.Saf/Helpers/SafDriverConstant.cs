namespace SeliseBlocks.Ecohub.Saf.Helpers;

public static class SafDriverConstant
{
    public const string GetReceiversEndpoint = "general/v1/saf-receivers";
    public const string TechUserEnrolmentEndpoint = "general/v1/techUserEnrolment";


    public const string GetAllPublicKeyEndpoint = "publickeystore/v1/keys";
    public const string GetPublicKeyDetailsEndpoint = "publickeystore/v1/keys/{keyId}";
    public const string GetPublicKeyByKeyTypeEndpoint = "publickeystore/v1/members/{idpNumber}/keys/{keyType}";
    public const string GetMemberPublicKeyEndpoint = "publickeystore/v1/members/{idpNumber}/keys";
    public const string GetEncryptedPublicKeyEndpoint = "publickeystore/v1/keys/{keyId}/verify";
    public const string UploadMemberPublicKeyEndpoint = "publickeystore/v1/keys";
    public const string VerifyDecryptedPublicKeyEndpoint = "publickeystore/v1/keys/{keyId}/verify";
    public const string ActivatePublicKeyEndpoint = "publickeystore/v1/keys/{keyId}/activate";
    public const string DeactivatePublicKeyEndpoint = "publickeystore/v1/keys/{keyId}/deactivate";
    public const string DeleteInactivePublicKeyEndpoint = "publickeystore/v1/keys/{keyId}";

    public const string SendOfferNlpiEventEndpoint = "saf/v1/in";
    public const string ReceiveOfferNlpiEventEndpoint = "saf/v1/{ecohubId}/offer/nlpi/out";
    public const string KafkaProducerTopic = "eh.saf.in.v1";
    public const string KafkaConsumerTopic = "eh.saf.{ecohubId}.offer.nlpi.out.v1";
    public const string SchemaRegistryUrl = "https://psrc-qrk9d.westeurope.azure.confluent.cloud:443";
    public const string SchemaRegistryAuth = "FCYTB2BG73BWKLZ5:juvZLo3Frvgoqn9Mb5dDJjaXx4NAYf1PwY+k5egoUBEHIYYCnmgzJE/M7uCCYjPv";
}

