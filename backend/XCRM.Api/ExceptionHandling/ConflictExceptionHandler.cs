using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using XCRM.Application.Common.Exceptions;

namespace XCRM.Api.ExceptionHandling
{
    internal sealed class ConflictExceptionHandler(ILogger<ConflictExceptionHandler> logger):IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ConflictException conflictException)
            {
                return false;
            }

            logger.LogWarning(exception, "请求发生业务冲突。TraceId:{TraceId}", httpContext.TraceIdentifier);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "请求冲突",
                Detail = conflictException.Message
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
