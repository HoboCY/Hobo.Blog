using System.Diagnostics;
using System.Linq;
using Blog.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.MVC.Controllers
{
    public class ErrorController : Controller
    {
        protected readonly ILogger<ErrorController> Logger;

        private static readonly int[] HandledHttpResponseCodes = { 404 };

        public ErrorController(ILogger<ErrorController> logger)
        {
            Logger = logger;
        }

        [Route("/error")]
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            if (statusCode.HasValue)
            {
                HttpContext.Response.StatusCode = statusCode.Value;

                if (HandledHttpResponseCodes.Contains(statusCode.Value))
                {
                    return File($"~/errorpages/{statusCode}.html", "text/html");
                }

                return StatusCode(statusCode.Value);
            }

            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                var routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                var exceptionThatOccurred = exceptionFeature.Error;
                Logger.LogError($"Error: {routeWhereExceptionOccurred}, " +
                                $"client IP: {HttpContext.Connection.RemoteIpAddress}, " +
                                $"request id: {requestId}", exceptionThatOccurred);
            }
            return View(new ErrorViewModel { RequestId = requestId });
        }

        [HttpGet]
        public IActionResult ServerError(string errorMessage=null)
        {
            return View(errorMessage);
        }
    }
}
