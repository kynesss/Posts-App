using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Posts.Errors;

internal static class FetchPostErrors
{
    public static readonly Error NotFound = new(
        "Posts.FetchPost.NotFound",
        "Post not found.",
        ErrorType.NotFound);

    public static readonly Error Unexpected = new(
        "Posts.FetchPost.Unexpected",
        "Unexpected error while loading post.",
        ErrorType.Failure);
}
