using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Features.Comments.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;
using LinqKit;

namespace PostsCommentsAPI.Features.Comments;

public static class GetPostComments
{
    public sealed record Query(int PostId, Filter Filter, Pager Pager) : IRequest<Result<Pagination<Response>>>;
    public sealed record Filter(string? Search = null);

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
                return Result.Failure<Pagination<Response>>(FetchPostCommentsErrors.PostNotFound);
            
            var predicate = PredicateBuilder.New<Comment>(x => x.PostId == request.PostId);

            if (!string.IsNullOrWhiteSpace(request.Filter?.Search))
            {
                predicate = predicate.And(x => x.Content.Contains(request.Filter.Search));
            }

            var pagedData = await dbContext.Comments
                .AsNoTracking()
                .Where(predicate)
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
