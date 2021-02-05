using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        private readonly IHostEnvironment env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            this.env = env;
            this.logger = logger;
            this.next = next;

        }

        public async Task InvokeAsync(HttpContext _context){
            try
            {
                await next(_context);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex,ex.Message);
                _context.Response.ContentType = "application/json";
                _context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = env.IsDevelopment() 
                ? new ApiException(_context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
                : new ApiException(_context.Response.StatusCode,"Internal Server Error");

                var options = new JsonSerializerOptions{
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(response, options);

                await _context.Response.WriteAsync(json);


               
            }
        }
    }
}