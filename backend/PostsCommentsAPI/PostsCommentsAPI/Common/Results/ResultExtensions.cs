namespace PostsCommentsAPI.Common.Results;

public static class ResultExtensions
{
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
    }

    public static T Match<TValue, T>(
        this Result<TValue> result,
        Func<TValue, T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess && result.Value is not null
            ? onSuccess(result.Value)
            : onFailure(result.Error);
    }

    public static IResult ToHttpResult(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => Microsoft.AspNetCore.Http.Results.BadRequest(new { error.Code, error.Message }),
            ErrorType.BadRequest => Microsoft.AspNetCore.Http.Results.BadRequest(new { error.Code, error.Message }),
            ErrorType.NotFound => Microsoft.AspNetCore.Http.Results.NotFound(new { error.Code, error.Message }),
            ErrorType.Conflict => Microsoft.AspNetCore.Http.Results.Conflict(new { error.Code, error.Message }),
            ErrorType.Unauthorized => Microsoft.AspNetCore.Http.Results.Unauthorized(),
            ErrorType.Forbidden => Microsoft.AspNetCore.Http.Results.StatusCode(StatusCodes.Status403Forbidden),
            _ => Microsoft.AspNetCore.Http.Results.Problem(detail: error.Message, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}