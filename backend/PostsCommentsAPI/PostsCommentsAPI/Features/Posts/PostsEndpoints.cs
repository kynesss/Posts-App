using MediatR;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Common.Results;
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
                var result = await mediator.Send(new GetPostList.Query(pager), cancellationToken);
                return result.Match(
                    onSuccess: value => Results.Ok(value),
                    onFailure: error => error.ToHttpResult());
            })
            .WithName("GetPostsList")
            .WithSummary("Gets all posts.");

        return endpoints;
    }
}
