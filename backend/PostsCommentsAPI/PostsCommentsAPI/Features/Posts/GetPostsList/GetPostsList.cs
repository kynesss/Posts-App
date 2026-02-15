using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Pagination;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Posts.GetPostsList;

public static class GetPostList
{
    public sealed record Query(Pager Pager) : IRequest<Pagination<Response>>;

    internal sealed class Handler(
        AppDbContext dbContext,
        IMapper mapper) : IRequestHandler<Query, Pagination<Response>>
    {
        public async Task<Pagination<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await dbContext.Posts
                .AsNoTracking()
                .Map<Post, Response>(mapper)
                .PaginateAsync(request.Pager, cancellationToken);
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