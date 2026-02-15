using FluentValidation;

namespace PostsCommentsAPI.Common.Filters;

public sealed class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
    where TRequest : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (request is null)
        {
            return await next(context);
        }

        var validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);
        if (!validationResult.IsValid)
        {
            return Microsoft.AspNetCore.Http.Results.ValidationProblem(validationResult.ToDictionary());
        }

        return await next(context);
    }
}
