using MediatR;
using PostsCommentsAPI.Common.Filters;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Posts;

public static class PostsEndpoints
{
    public static void MapPostEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/posts", async (
                [AsParameters] GetPostList.Filter filter,
                [AsParameters] Pager pager,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetPostList.Query(filter, pager), cancellationToken);

                return result.Match(
                    onSuccess: Results.Ok,
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
                    onSuccess: Results.Ok,
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

        endpoints.MapPut("/posts/{id:int}", async (
                int id,
                UpdatePost.Request request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(
                    new UpdatePost.Command(id, request.Title, request.Content),
                    cancellationToken);

                return result.Match(
                    onSuccess: Results.NoContent,
                    onFailure: error => error.ToHttpResult());
            })
            .AddEndpointFilter<ValidationFilter<UpdatePost.Request>>()
            .WithName("UpdatePost")
            .WithSummary("Updates an existing post.");

        endpoints.MapDelete("/posts/{id:int}", async (
                int id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new DeletePost.Command(id), cancellationToken);

                return result.Match(
                    onSuccess: Results.NoContent,
                    onFailure: error => error.ToHttpResult());
            })
            .WithName("DeletePost")
            .WithSummary("Deletes post by id.");
    }
}
