using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Features.Posts.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Posts;

public static class UpdatePost
{
    public sealed record Request(string? Title, string? Content);

    internal sealed record Command(int Id, string? Title, string? Content) : IRequest<Result>;

    internal sealed class Handler(AppDbContext dbContext, IMapper mapper)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var post = await dbContext.Posts
                .FirstOrDefaultAsync(post => post.Id == request.Id, cancellationToken);

            if (post is null)
            {
                return Result.Failure(UpdatePostErrors.NotFound);
            }

            mapper.Map(request, post);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Command, Post>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());
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
