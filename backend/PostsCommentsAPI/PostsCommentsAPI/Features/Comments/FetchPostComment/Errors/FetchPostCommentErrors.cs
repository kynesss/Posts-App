using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Comments.Errors;

internal static class FetchPostCommentErrors
{
    public static readonly Error NotFound = new(
        "Comments.FetchPostComment.NotFound",
        "Comment not found.",
        ErrorType.NotFound);
}
