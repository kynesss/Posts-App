using MediatR;
using PostsCommentsAPI.Common.Filters;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Posts;

public static class PostsEndpoints
{
    public static IEndpointRouteBuilder MapPostEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/posts", async (
                [AsParameters] Pager pager,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetPostList.Query(pager), cancellationToken);

                return result.Match(
                    onSuccess: value => Results.Ok(value),
                    onFailure: error => error.ToHttpResult());
            })
            .WithName("GetPostsList")
            .WithSummary("Gets all posts.");

        endpoints.MapGet("/posts/{id:int}", async (
                int id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new FetchPost.Query(id), cancellationToken);

                return result.Match(
                    onSuccess: value => Results.Ok(value),
                    onFailure: error => error.ToHttpResult());
            })
            .WithName("FetchPost")
            .WithSummary("Gets post details by id.");

        endpoints.MapPost("/posts", async (
                CreatePost.Request request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(
                    new CreatePost.Command(request.Title, request.Content),
                    cancellationToken);

                return result.Match(
                    onSuccess: () => Results.StatusCode(StatusCodes.Status201Created),
                    onFailure: error => error.ToHttpResult());
            })
            .AddEndpointFilter<ValidationFilter<CreatePost.Request>>()
            .WithName("CreatePost")
            .WithSummary("Creates a new post.");

        return endpoints;
    }
}
