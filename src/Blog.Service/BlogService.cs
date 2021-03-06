﻿using System;
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
            var result = Guid.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue("UserId"), out Guid userId);
            if (!result)
            {
                throw new ArgumentNullException("UserId Not Found");
            }
            return userId;
        }
    }
}
