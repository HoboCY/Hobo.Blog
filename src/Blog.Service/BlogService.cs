using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Blog.Service
{
    public class BlogService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected Guid UserId()
        {
            var result = Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserId"), out var userId);
            return !result ? throw new ArgumentNullException(nameof(UserId), "UserId Not Found") : userId;
        }
    }
}
