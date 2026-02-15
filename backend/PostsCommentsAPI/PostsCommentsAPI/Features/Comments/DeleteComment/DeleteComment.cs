using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Features.Comments.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Comments;

public static class DeleteComment
{
    internal sealed record Command(int Id) : IRequest<Result>;

    internal sealed class Handler(AppDbContext dbContext)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var comment = await dbContext.Comments
                .FirstOrDefaultAsync(comment => comment.Id == request.Id, cancellationToken);

            if (comment is null)
            {
                return Result.Failure(DeleteCommentErrors.NotFound);
            }

            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
