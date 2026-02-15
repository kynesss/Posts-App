using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Posts.GetPostsList.Errors;

internal static class GetPostListErrors
{
    public static readonly Error Unexpected = new(
        "Posts.GetPostList.Unexpected",
        "Unexpected error while loading posts.",
        ErrorType.Failure);
}
