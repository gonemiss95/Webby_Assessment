using FluentValidation;

namespace UserManagement.Application.Posts.Comands.UpdatePostCommand
{
    public class UpdatePostValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostValidator()
        {
            RuleFor(x => x.PostId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Post Id cannot be empty.")
                .GreaterThan(0).WithMessage("Post Id cannot be less than 0.");

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
