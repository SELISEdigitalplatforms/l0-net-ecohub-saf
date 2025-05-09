namespace SeliseBlocks.Ecohub.Saf;

public class SafOfferNlpiEvent : BaseSafOfferNlpiEvent
{
    public SafData Data { get; set; }
}

public class SafData
{
    public byte[] Payload { get; set; }
    public string PublicKey { get; set; }
    public List<SafLinks> Links { get; set; }
    public string PublicKeyVersion { get; set; }
    public string Message { get; set; }
}
