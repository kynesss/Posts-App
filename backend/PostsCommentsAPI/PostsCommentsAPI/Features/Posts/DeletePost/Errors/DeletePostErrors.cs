using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Posts.Errors;

internal static class DeletePostErrors
{
    public static readonly Error NotFound = new(
        "Posts.DeletePost.NotFound",
        "Post not found.",
        ErrorType.NotFound);
}
