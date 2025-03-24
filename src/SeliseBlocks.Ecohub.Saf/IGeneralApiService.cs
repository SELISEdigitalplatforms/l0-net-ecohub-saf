using System;

namespace SeliseBlocks.Ecohub.Saf;

public interface IGeneralApiService
{
    Task RetrieveTechUserCredentials();
    Task RetrieveSafReceivers();
    Task RetrieveSafInsurers();
}
