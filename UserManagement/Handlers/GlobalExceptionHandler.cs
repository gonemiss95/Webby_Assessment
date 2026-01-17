using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails details = new ProblemDetails();

            if (exception is ValidationException validationEx)
            {
                details = new ProblemDetails
                {
                    Title = "Validation Failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "One or more validation errors occurred",
                    Extensions = new Dictionary<string, object?>()
                    {
                        {
                            "errors",
                            validationEx.Errors.Select(x => new
                            {
                                field = x.PropertyName,
                                message = x.ErrorMessage
                            })
                        }
                    }
                };
            }
            else
            {
                details = new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "An internal server error occured."
                };
            }

            httpContext.Response.StatusCode = details.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
            return true;
        }
    }
}
