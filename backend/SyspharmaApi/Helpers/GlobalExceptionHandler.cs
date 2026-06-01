using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyspharmaApi.Context;

namespace SyspharmaApi.Helpers
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,IServiceScopeFactory scopeFactory): IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<SyspharmaContext>();

                await context.Errors.AddAsync(new Models.Error
                {
                    Title = httpContext.Request.Path,
                    Description = exception.ToString(),
                    ErrorAt = DateTime.Now
                }, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logException)
            {
                logger.LogError(logException,"Failed to save exception to database.");
            }

            logger.LogError(
                exception,
                "Unhandled exception while processing request.");

            var problemDetails = new ProblemDetails
            {
                Title = "An unexpected error occurred.",
                Detail = "The request could not be processed.",
                Status = StatusCodes.Status500InternalServerError,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }
    }
}
