using MediatR;
using PostsCommentsAPI.Common.Filters;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Comments;

public static class CommentsEndpoints
{
    public static IEndpointRouteBuilder MapCommentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/posts/{postId:int}/comments", async (
                int postId,
                [AsParameters] Pager pager,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new FetchPostComments.Query(postId, pager), cancellationToken);

                return result.Match(
                    onSuccess: value => Results.Ok(value),
                    onFailure: error => error.ToHttpResult());
            })
            .WithName("FetchPostComments")
            .WithSummary("Gets comments for a specific post.");

        endpoints.MapPost("/posts/{postId:int}/comments", async (
                int postId,
                CreateComment.Request request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(
                    new CreateComment.Command(postId, request.Content),
                    cancellationToken);

                return result.Match(
                    onSuccess: () => Results.StatusCode(StatusCodes.Status201Created),
                    onFailure: error => error.ToHttpResult());
            })
            .AddEndpointFilter<ValidationFilter<CreateComment.Request>>()
            .WithName("CreateComment")
            .WithSummary("Creates a comment for a specific post.");

        endpoints.MapPut("/comments/{id:int}", async (
                int id,
                UpdateComment.Request request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(
                    new UpdateComment.Command(id, request.Content),
                    cancellationToken);

                return result.Match(
                    onSuccess: () => Results.NoContent(),
                    onFailure: error => error.ToHttpResult());
            })
            .AddEndpointFilter<ValidationFilter<UpdateComment.Request>>()
            .WithName("UpdateComment")
            .WithSummary("Updates comment by id.");

        endpoints.MapDelete("/comments/{id:int}", async (
                int id,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new DeleteComment.Command(id), cancellationToken);

                return result.Match(
                    onSuccess: () => Results.NoContent(),
                    onFailure: error => error.ToHttpResult());
            })
            .WithName("DeleteComment")
            .WithSummary("Deletes comment by id.");

        return endpoints;
    }
}
