using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Comments.Errors;

internal static class UpdateCommentErrors
{
    public static readonly Error NotFound = new(
        "Comments.UpdateComment.NotFound",
        "Comment not found.",
        ErrorType.NotFound);
}
