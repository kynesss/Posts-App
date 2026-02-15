using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Features.Posts.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Posts;

public static class FetchPost
{
    public sealed record Query(int Id) : IRequest<Result<Response>>;

    public sealed record Response(int Id, string Title, string? Content, DateTime CreatedAt);

    internal sealed class Handler(AppDbContext dbContext, IMapper mapper)
        : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            try
            {
                var post = await dbContext.Posts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

                if (post is null)
                {
                    return Result.Failure<Response>(FetchPostErrors.NotFound);
                }

                return Result.Success(mapper.Map<Response>(post));
            }
            catch (Exception)
            {
                return Result.Failure<Response>(FetchPostErrors.Unexpected);
            }
        }
    }

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, Response>();
        }
    }
}
