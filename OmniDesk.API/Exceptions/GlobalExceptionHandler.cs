using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OmniDesk.Application.Common.Exceptions;

namespace OmniDesk.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = exception switch
        {
            ValidationException validationEx => new ValidationProblemDetails(validationEx.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Lỗi kiểm tra dữ liệu",
                Detail = validationEx.Message
            },
            NotFoundException notFoundEx => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Không tìm thấy dữ liệu",
                Detail = notFoundEx.Message
            },
            ConflictException conflictEx => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Xung đột dữ liệu",
                Detail = conflictEx.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Lỗi hệ thống",
                Detail = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại sau."
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}