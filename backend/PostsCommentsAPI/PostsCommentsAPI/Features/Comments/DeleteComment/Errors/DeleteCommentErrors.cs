using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Comments.Errors;

internal static class DeleteCommentErrors
{
    public static readonly Error NotFound = new(
        "Comments.DeleteComment.NotFound",
        "Comment not found.",
        ErrorType.NotFound);
}
