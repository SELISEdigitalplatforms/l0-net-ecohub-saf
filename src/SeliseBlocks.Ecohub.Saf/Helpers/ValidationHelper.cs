using System;
using System.ComponentModel.DataAnnotations;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

public static class ValidationHelper
{
    public static SafValidationResponse Validate<TRequest>(this TRequest request) where TRequest : class
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();
        var response = new SafValidationResponse
        {
            IsSuccess = true
        };
        if (!Validator.TryValidateObject(request, context, results, validateAllProperties: true))
        {
            var errorMessages = string.Join("; ", results.Select(r => r.ErrorMessage));
            response.IsSuccess = false;
            response.Error = new SafError
            {
                ErrorCode = "ValidationError",
                ErrorMessage = errorMessages
            };
        }
        return response;
    }

}
