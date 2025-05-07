namespace SeliseBlocks.Ecohub.Saf.Helpers;

public static class SafDriverConstant
{
    public const string GetReceiversEndpoint = "general/saf-receivers";
    public const string TechUserEnrolmentEndpoint = "general/techUserEnrolment";
    public const string UploadMemberPublicKeyEndpoint = "publickeystore/keys";
    public const string GetMemberPublicKeyEndpoint = "publickeystore/members/{idpNumber}/key";
    public const string GetEncryptedPublicKeyEndpoint = "publickeystore/keys/{keyId}/verify";
    public const string VerifyDecryptedPublicKeyEndpoint = "publickeystore/keys/{keyId}/verify";
    public const string ActivatePublicKeyEndpoint = "publickeystore/keys/{keyId}/activate";
    public const string SendOfferNlpiEventEndpoint = "saf/in";
    public const string ReceiveOfferNlpiEventEndpoint = "saf/{ecohubId}/offer/nlpi/out";
}
