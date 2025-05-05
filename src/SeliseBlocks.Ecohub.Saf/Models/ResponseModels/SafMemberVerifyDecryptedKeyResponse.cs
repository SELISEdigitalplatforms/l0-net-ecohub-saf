namespace SeliseBlocks.Ecohub.Saf;

public class SafMemberVerifyDecryptedKeyResponse
{
    public string VerificationStatus { get; set; } = string.Empty;
}

public static class VerificationStatus
{
    public const string Success = "Success";
    public const string Fail = "Fail";
}
