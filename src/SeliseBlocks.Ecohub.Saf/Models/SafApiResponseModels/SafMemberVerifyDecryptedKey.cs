namespace SeliseBlocks.Ecohub.Saf;

public class SafMemberVerifyDecryptedKey
{
    public string VerificationStatus { get; set; } = string.Empty;
}

public static class VerificationStatus
{
    public const string Success = "Success";
    public const string Fail = "Fail";
}
