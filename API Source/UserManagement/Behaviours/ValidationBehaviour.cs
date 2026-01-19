using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace UserManagement.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
                ValidationResult[] resultArr = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));
                List<ValidationFailure> failList = resultArr.Where(x => x != null).SelectMany(x => x.Errors).ToList();

                if (failList.Count != 0)
                {
                    throw new ValidationException(failList);
                }
            }

            return await next(cancellationToken);
        }
    }
}
