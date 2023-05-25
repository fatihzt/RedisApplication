using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace RedisApplication.Api.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly string _logErrorFilePath;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _logErrorFilePath = "C:\\Users\\muham\\source\\Repos\\RedisApplication\\RedisApplication.Core\\Log\\ErrorLog.txt";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await WriteErrorLog(ex);
                await HandleExceptionAsync(context, ex);
            }
            
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            ProblemDetails problem = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Detail = ex.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }

        private async Task WriteErrorLog(Exception ex)
        {
            string log = $"[{DateTime.UtcNow}] Error: {ex.Message}\nStack Trace: {ex.StackTrace}\n";
            await using (var writer = new StreamWriter(_logErrorFilePath, true))
            {
                await writer.WriteLineAsync(log);
            }
        }
    }
}

