namespace SeliseBlocks.Ecohub.Saf.Helpers;

public static class SafDriverConstant
{
    public const string GetReceiversEndpoint = "general/saf-receivers";
    public const string TechUserEnrolmentEndpoint = "general/techUserEnrolment";


    public const string GetAllPublicKeyEndpoint = "publickeystore/keys";
    public const string GetPublicKeyDetailsEndpoint = "publickeystore/keys/{keyId}";
    public const string GetPublicKeyByKeyTypeEndpoint = "publickeystore/members/{idpNumber}/keys";
    public const string GetMemberPublicKeyEndpoint = "publickeystore/members/{idpNumber}/key";
    public const string GetEncryptedPublicKeyEndpoint = "publickeystore/keys/{keyId}/verify";
    public const string UploadMemberPublicKeyEndpoint = "publickeystore/keys";
    public const string VerifyDecryptedPublicKeyEndpoint = "publickeystore/keys/{keyId}/verify";
    public const string ActivatePublicKeyEndpoint = "publickeystore/keys/{keyId}/activate";
    public const string DeactivatePublicKeyEndpoint = "publickeystore/keys/{keyId}/deactivate";
    public const string DeleteInactivePublicKeyEndpoint = "publickeystore/keys/{keyId}";

    public const string SendOfferNlpiEventEndpoint = "saf/in";
    public const string ReceiveOfferNlpiEventEndpoint = "saf/{ecohubId}/offer/nlpi/out";
    public const string KafkaProducerTopic = "eh.saf.in";
    public const string KafkaConsumerTopic = "eh.saf.{ecohubId}.offer.nlpi.out";
    public const string SchemaRegistryUrl = "https://psrc-qrk9d.westeurope.azure.confluent.cloud:443";
    public const string SchemaRegistryAuth = "FCYTB2BG73BWKLZ5:juvZLo3Frvgoqn9Mb5dDJjaXx4NAYf1PwY+k5egoUBEHIYYCnmgzJE/M7uCCYjPv";
}

