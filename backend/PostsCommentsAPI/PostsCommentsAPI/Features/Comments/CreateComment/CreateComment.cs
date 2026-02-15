using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Features.Comments.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Comments;

public static class CreateComment
{
    public sealed record Request(string? Content);

    internal sealed record Command(int PostId, string? Content) : IRequest<Result>;

    internal sealed class Handler(AppDbContext dbContext, IMapper mapper)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var postExists = await dbContext.Posts
                .AsNoTracking()
                .AnyAsync(post => post.Id == request.PostId, cancellationToken);

            if (!postExists)
            {
                return Result.Failure(CreateCommentErrors.PostNotFound);
            }

            var comment = mapper.Map<Comment>(request);

            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Command, Comment>();
        }
    }

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}
