using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace DreamCine.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                int statusCode = (int)HttpStatusCode.InternalServerError;
                string message = "An unexpected error occurred on the server side. Please try again later.";

                if (ex is DbUpdateException && ex.InnerException?.Message.Contains("IX_Tickets_MovieSessionId_SeatId") == true)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "This seat already reserved for selected session. Please select another seat.";
                }

                _logger.LogError(ex, "An uncaught error occurred in the system!");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = new
                {
                    StatusCode = statusCode,
                    Message = message,
                    Detailed = _env.IsDevelopment() ? ex.Message : null
                };

                var jsonResponse = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
