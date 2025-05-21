using System;

namespace SeliseBlocks.Ecohub.Saf;

public class SafBaseResponse<T> : SafValidationResponse
{
    public T? Data { get; set; }
}
