using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Exceptions;
using Blog.Service.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Blog.Permissions.AuthorizationHandlers
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IUserService _userService;

        public PermissionAuthorizationHandler(IUserService userService)
        {
            _userService = userService;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            var roles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!roles.Any())
            {
                throw new BlogException(StatusCodes.Status403Forbidden, "操作失败，当前用户没有权限");
            }

            if (await _userService.CheckAsync(requirement.Name, roles))
            {
                context.Succeed(requirement);
                return;
            }

            throw new BlogException(StatusCodes.Status403Forbidden, "操作失败，当前用户没有权限");
        }
    }
}
