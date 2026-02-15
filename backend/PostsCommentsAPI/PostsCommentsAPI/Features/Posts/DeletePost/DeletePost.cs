using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Features.Posts.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Posts;

public static class DeletePost
{
    internal sealed record Command(int Id) : IRequest<Result>;

    internal sealed class Handler(AppDbContext dbContext)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = await dbContext.Posts
                .FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

            if (post is null)
            {
                return Result.Failure(DeletePostErrors.NotFound);
            }

            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
