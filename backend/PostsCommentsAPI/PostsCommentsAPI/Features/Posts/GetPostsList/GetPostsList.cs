using AutoMapper;
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
    public sealed record Query(Pager Pager) : IRequest<Result<Pagination<Response>>>;

    internal sealed class Handler(
        AppDbContext dbContext,
        IMapper mapper) : IRequestHandler<Query, Result<Pagination<Response>>>
    {
        public async Task<Result<Pagination<Response>>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var pagedData = await dbContext.Posts
                    .AsNoTracking()
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