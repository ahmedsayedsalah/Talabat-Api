using Talabat.API.Errors;
using System.Net;
using System.Text.Json;

namespace Talabat.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context); // next.Invoke(contxet);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                var statusCode= context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = (env.IsDevelopment()) ?
                    new ApiExceptionResponse(statusCode, ex.Message, ex.StackTrace.ToString())
                    : new ApiExceptionResponse(statusCode, ex.Message, ex.StackTrace.ToString());

                // to convert to camelCase
                var options=new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json= JsonSerializer.Serialize(response,options);

                await context.Response.WriteAsync(json);

            }

        }
    }
}
