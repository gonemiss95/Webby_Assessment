using FluentValidation;

namespace UserManagement.Application.Posts.Comands.CreatePostCommand
{
    public class CreatePostValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostValidator()
        {
            RuleFor(x => x.PostAbbr)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Post abbreviation cannot be empty.")
                .Matches("^[a-zA-Z0-9]+$").WithMessage("Post abbreviation cannot have any spaces or special characters.")
                .MaximumLength(20).WithMessage("Post abbreviation cannot exceed 20 character.");

            RuleFor(x => x.PostTitle)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Post Title cannot be empty.")
                .MinimumLength(5).WithMessage("Post Title must contains at least 5 characters.")
                .MaximumLength(100).WithMessage("Post Title cannot exceed 100 character.");

            RuleFor(x => x.TagIdList)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tag Id cannot be empty.");
        }
    }
}
