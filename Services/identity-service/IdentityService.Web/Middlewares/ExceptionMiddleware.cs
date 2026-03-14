using IdentityService.Application.Common.TemplateResponses;
using System.Net;
using System.Text.Json;

namespace IdentityService.Web.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                                 ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            // Mặc định là lỗi 500
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = "An internal server error occurred.";

            // Bạn có thể phân loại Exception để trả về Status Code phù hợp
            if (ex is UnauthorizedAccessException) statusCode = (int)HttpStatusCode.Unauthorized;
            // if (ex is MyCustomNotFoundException) statusCode = (int)HttpStatusCode.NotFound;

            context.Response.StatusCode = statusCode;

            // Trong môi trường Development, trả về lỗi chi tiết. 
            // Trong Production, chỉ trả về message chung chung để bảo mật.
            var response = ApiResponse<object>.FailResponse(
                _env.IsDevelopment() ? $"{ex.Message} {ex.StackTrace}" : message
            );

            JsonSerializerOptions jsonSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            JsonSerializerOptions options = jsonSerializerOptions;
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
    }
}
