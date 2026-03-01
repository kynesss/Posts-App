using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Features.Posts.GetPostsList.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Posts;

public static class GetPostList
{
    public sealed record Query(Filter Filter, Pager Pager) : IRequest<Result<Pagination<Response>>>;

    public sealed record Filter(string? Search = null);

    internal sealed class Handler(
        AppDbContext dbContext,
        IMapper mapper) : IRequestHandler<Query, Result<Pagination<Response>>>
    {
        public async Task<Result<Pagination<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var predicate = PredicateBuilder.New<Post>(true);

                if (!string.IsNullOrWhiteSpace(request.Filter?.Search))
                {
                    predicate = predicate.And(s => s.Title.Contains(request.Filter.Search));
                }
                
                var pagedData = await dbContext.Posts
                    .AsNoTracking()
                    .Where(predicate)
                    .Map<Post, Response>(mapper)
                    .PaginateAsync(request.Pager, cancellationToken);

                return Result.Success(pagedData);
            }
            catch (Exception)
            {
                return Result.Failure<Pagination<Response>>(GetPostListErrors.Unexpected);
            }
        }
    }

    internal sealed record Response(int Id, string Title, string? Content, DateTime CreatedAt);

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, Response>();
        }
    }
}