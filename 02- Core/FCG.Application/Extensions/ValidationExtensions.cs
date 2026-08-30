using FCG.Domain.Commons;
using FCG.Domain.Commons.Result;
using FluentValidation.Results;

namespace FCG.Application.Extensions;

public static class ValidationExtensions
{
    public static InvalidResult<T>? ToInvalidResult<T>(this ValidationResult validationResult)
    {
        if (validationResult.IsValid)
        {
            return null;
        }

        var errors = validationResult.Errors
            .Select(e => new Error(e.ErrorMessage, e.PropertyName));

        return InvalidResult<T>.Create(errors);
    }
}
