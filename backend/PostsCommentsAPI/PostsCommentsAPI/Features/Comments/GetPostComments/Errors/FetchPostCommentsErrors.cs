using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Comments.Errors;

internal static class FetchPostCommentsErrors
{
    public static readonly Error PostNotFound = new(
        "Comments.FetchPostComments.PostNotFound",
        "Post not found.",
        ErrorType.NotFound);
}
