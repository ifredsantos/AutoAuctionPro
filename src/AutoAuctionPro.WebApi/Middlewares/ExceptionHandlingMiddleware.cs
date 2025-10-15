using AutoAuctionPro.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AutoAuctionPro.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int status;
            string title;
            string detail;

            if (ex is IDomainException domainEx)
            {
                status = domainEx.StatusCode;
                title = domainEx.Title;
                detail = ex.Message;

                _logger.LogWarning("Domain exception: {Message}", ex.Message);
            }
            else if (ex is ArgumentException argException)
            {
                status = 400;
                title = "Argument exception";
                detail = ex.Message;

                _logger.LogWarning(ex.Message);
            }
            else
            {
                status = StatusCodes.Status500InternalServerError;
                title = "Unexpected error";
                detail = "An unexpected error occurred. Please contact support if the problem persists.";
            }

            ProblemDetails problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
