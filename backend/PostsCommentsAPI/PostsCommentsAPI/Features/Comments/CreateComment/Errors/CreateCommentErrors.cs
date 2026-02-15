using PostsCommentsAPI.Common.Results;

namespace PostsCommentsAPI.Features.Comments.Errors;

internal static class CreateCommentErrors
{
    public static readonly Error PostNotFound = new(
        "Comments.CreateComment.PostNotFound",
        "Post not found.",
        ErrorType.NotFound);
}
