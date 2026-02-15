using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostsCommentsAPI.Common.Results;
using PostsCommentsAPI.Domain.Entities;
using PostsCommentsAPI.Features.Comments.Errors;
using PostsCommentsAPI.Infrastructure.Persistence;

namespace PostsCommentsAPI.Features.Comments;

public static class UpdateComment
{
    public sealed record Request(string? Content);

    internal sealed record Command(int Id, string? Content) : IRequest<Result>;

    internal sealed class Handler(AppDbContext dbContext, IMapper mapper)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            var comment = await dbContext.Comments
                .FirstOrDefaultAsync(comment => comment.Id == request.Id, cancellationToken);

            if (comment is null)
            {
                return Result.Failure(UpdateCommentErrors.NotFound);
            }

            mapper.Map(request, comment);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Command, Comment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostId, opt => opt.Ignore())
                .ForMember(dest => dest.Post, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore());
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
