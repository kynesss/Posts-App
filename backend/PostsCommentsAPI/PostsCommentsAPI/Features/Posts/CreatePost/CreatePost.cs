using AutoMapper;
using FluentValidation;
using MediatR;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Posts;

public static class CreatePost
{
    public sealed record Request(string? Title, string? Content);

    internal sealed record Command(Request Request) : IRequest<Result>;

    internal sealed class Handler(AppDbContext dbContext, IMapper mapper)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var post = mapper.Map<Post>(command.Request);

            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Request, Post>();
        }
    }

    public sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(5000);
        }
    }
}
