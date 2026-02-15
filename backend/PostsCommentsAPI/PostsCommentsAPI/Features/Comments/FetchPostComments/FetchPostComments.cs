using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Features.Comments.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Comments;

public static class FetchPostComments
{
    public sealed record Query(int PostId, Pager Pager) : IRequest<Result<Pagination<Response>>>;

    public sealed record Response(int Id, int PostId, string Content, DateTime CreatedAt);

    internal sealed class Handler(AppDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, Result<Pagination<Response>>>
    {
        public async Task<Result<Pagination<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var postExists = await dbContext.Posts
                .AsNoTracking()
                .AnyAsync(post => post.Id == request.PostId, cancellationToken);

            if (!postExists)
            {
                return Result.Failure<Pagination<Response>>(FetchPostCommentsErrors.PostNotFound);
            }

            var pagedData = await dbContext.Comments
                .AsNoTracking()
                .Where(comment => comment.PostId == request.PostId)
                .Map<Comment, Response>(mapper)
                .PaginateAsync(request.Pager, cancellationToken);

            return Result.Success(pagedData);
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
