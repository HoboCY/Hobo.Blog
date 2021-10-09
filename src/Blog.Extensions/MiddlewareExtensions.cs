using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Blog.Exceptions;
using Blog.Shared;

namespace Blog.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app, RequestDelegate exceptionHandler)
        {
            return app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = exceptionHandler
            });
        }

        public static async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8";
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var ex = exceptionHandlerPathFeature?.Error;
            if (ex != null)
            {
                var exception = ex as BlogException;

                if (exception == null && context.Response.StatusCode is StatusCodes.Status500InternalServerError)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync($"系统异常，请邮件联系管理员：{BlogConstants.AdministratorsEmail}");
                    return;
                }

                if (exception != null)
                {
                    context.Response.StatusCode = exception.StatusCode;
                    await context.Response.WriteAsync(exception.Message);
                }
            }
        }
    }
}
