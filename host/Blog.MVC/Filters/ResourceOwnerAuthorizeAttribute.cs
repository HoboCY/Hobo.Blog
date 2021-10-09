using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Exceptions;
using Blog.Service.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.MVC.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ResourceOwnerAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var postService = context.HttpContext.RequestServices.GetRequiredService<IPostService>();

            var creatorId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            var resourceId = context.HttpContext.Request.RouteValues["id"] ?? throw new BadHttpRequestException("Invalid request params");

            var resource = await postService.GetPostAsync(resourceId.ToString());

            if (creatorId?.Value != resource.CreatorId)
            {
                throw new BlogException(StatusCodes.Status403Forbidden, "操作失败，该操作只有作者拥有权限");
            }

            await next.Invoke();
        }
    }
}
