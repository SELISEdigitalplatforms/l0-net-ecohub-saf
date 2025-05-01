using System;
using System.ComponentModel.DataAnnotations;

namespace SeliseBlocks.Ecohub.Saf.Helpers;

public static class ValidationHelper
{
    public static void Validate<TRequest>(this TRequest request) where TRequest : class
    {
        var context = new ValidationContext(request);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(request, context, results, validateAllProperties: true))
        {
            var errorMessages = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new ValidationException($"Invalid person request: {errorMessages}");
        }
    }

}
