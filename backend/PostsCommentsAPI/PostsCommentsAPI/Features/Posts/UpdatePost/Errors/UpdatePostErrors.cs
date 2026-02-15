using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Posts.Errors;

internal static class UpdatePostErrors
{
    public static readonly Error NotFound = new(
        "Posts.UpdatePost.NotFound",
        "Post not found.",
        ErrorType.NotFound);
}
