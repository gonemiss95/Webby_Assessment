using FluentValidation;

namespace UserManagement.Application.Tags.Queries.GetTagListPagination
{
    public class GetTagListPaginationValidator : AbstractValidator<GetTagListPaginationQuery>
    {
        public GetTagListPaginationValidator()
        {
            RuleFor(x => x.PageNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Page Number cannot be empty.")
                .GreaterThan(0).WithMessage("Page Number cannot be less than 0.");

            RuleFor(x => x.PageSize)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Page Number cannot be empty.")
                .GreaterThan(0).WithMessage("Page Number cannot be less than 0.");
        }
    }
}
