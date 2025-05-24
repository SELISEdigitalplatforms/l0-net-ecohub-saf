using System;

namespace SeliseBlocks.Ecohub.Saf;

public class SafRestProxyBaseResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public SafRestProxyError? Error { get; set; }
}
