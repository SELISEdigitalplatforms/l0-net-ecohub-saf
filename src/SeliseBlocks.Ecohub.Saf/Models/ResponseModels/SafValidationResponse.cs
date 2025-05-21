using System;

namespace SeliseBlocks.Ecohub.Saf;

public class SafValidationResponse
{
    public bool IsSuccess { get; set; }
    public SafError? Error { get; set; }

}
