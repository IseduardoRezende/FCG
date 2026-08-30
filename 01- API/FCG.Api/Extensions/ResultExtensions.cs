using FCG.Domain.Commons.Result;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, int successStatusCode = StatusCodes.Status200OK)
    {
        return result switch
        {
            SuccessResult<T> => successStatusCode switch
            {
                StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = successStatusCode },
                StatusCodes.Status204NoContent => new StatusCodeResult(successStatusCode),
                _ => new OkObjectResult(result)
            },
            InvalidResult<T> => new UnprocessableEntityObjectResult(result),
            NotFoundResult<T> => new NotFoundObjectResult(result),
            ConflictResult<T> => new ConflictObjectResult(result),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}
