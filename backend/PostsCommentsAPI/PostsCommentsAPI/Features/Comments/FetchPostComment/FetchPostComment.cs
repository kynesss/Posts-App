using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Features.Comments.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Comments;

public static class FetchPostComment
{
    public sealed record Query(int PostId, int Id) : IRequest<Result<Response>>;

    public sealed record Response(int Id, int PostId, string Content, DateTime CreatedAt);

    internal sealed class Handler(AppDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comment = await dbContext.Comments
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    comment => comment.PostId == request.PostId && comment.Id == request.Id,
                    cancellationToken);

            if (comment is null)
            {
                return Result.Failure<Response>(FetchPostCommentErrors.NotFound);
            }

            return Result.Success(mapper.Map<Response>(comment));
        }
    }

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, Response>();
        }
    }
}
