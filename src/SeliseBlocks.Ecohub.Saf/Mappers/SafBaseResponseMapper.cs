using System;

namespace SeliseBlocks.Ecohub.Saf;

public static class SafBaseResponseMapper
{
    public static TResponse MapToDerivedResponse<TModel, TResponse>(this SafBaseResponse<TModel> baseResponse)
        where TModel : class
        where TResponse : SafBaseResponse<TModel>, new()
    {
        return new TResponse
        {
            IsSuccess = baseResponse.IsSuccess,
            Error = baseResponse.Error,
            Data = baseResponse.Data
        };
    }

    public static TResponse MapToDerivedResponse<TModel, TResponse>(this SafBaseResponse<IEnumerable<TModel>> baseResponse)
        where TModel : class
        where TResponse : SafBaseResponse<IEnumerable<TModel>>, new()
    {
        if (baseResponse == null)
        {
            throw new ArgumentNullException(nameof(baseResponse), "Base response cannot be null");
        }

        return new TResponse
        {
            IsSuccess = baseResponse.IsSuccess,
            Error = baseResponse.Error,
            Data = baseResponse.Data
        };
    }
    public static TResponse MapToResponse<TModel, TResponse>(this SafValidationResponse validationResponse)
        where TModel : class
        where TResponse : SafBaseResponse<TModel>, new()
    {
        return new TResponse
        {
            IsSuccess = validationResponse.IsSuccess,
            Error = validationResponse.Error
        };
    }
}
