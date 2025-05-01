using System;

namespace SeliseBlocks.Ecohub.Saf;

public static class SafDriverConstant
{
    public const string GetReceiversEndpoint = "general/saf-receivers";
    public const string TechUserEnrolmentEndpoint = "general/techUserEnrolment";
    public const string GetMemberPublicKeyEndpoint = "publickeystore/members/{idpNumber}/key";
    public const string SendOfferNlpiEventEndpoint = "saf/in";
    public const string ReceiveOfferNlpiEventEndpoint = "saf/{ecohubId}/offer/nlpi/out";


}
