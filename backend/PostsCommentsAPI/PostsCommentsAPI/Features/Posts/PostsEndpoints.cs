using MediatR;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Features.Posts.GetPostsList;

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
                var posts = await mediator.Send(new GetPostList.Query(pager), cancellationToken);
                return Results.Ok(posts);
            })
            .WithName("GetPostsList")
            .WithSummary("Gets all posts.");

        return endpoints;
    }
}