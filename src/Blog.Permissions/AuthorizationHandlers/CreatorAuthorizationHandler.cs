using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Blog.Data;

namespace Blog.Permissions.AuthorizationHandlers
{
    public class CreatorAuthorizationHandler<T> : AuthorizationHandler<OperationAuthorizationRequirement, T> where T : ICreator<string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            T resource)
        {

            var id = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (id?.Value == resource.CreatorId)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
