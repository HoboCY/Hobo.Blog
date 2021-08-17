using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data
{
    public static class SqlConstants
    {
        public const string GetCategory = @"SELECT * FROM category WHERE Id = @Id AND IsDeleted = 0";

        public const string GetCategories = @"SELECT * FROM category WHERE IsDeleted = 0";

        public const string GetPost = @"SELECT * FROM post WHERE Id = @Id AND CreatorId = @CreatorId AND IsDeleted = 0";

        public const string GetPosts =
            @"SELECT p.*,u.UserName AS CreatorName FROM post p LEFT JOIN application_user u ON p.CreatorId = u.Id WHERE p.IsDeleted = 0 ORDER BY p.CreationTime DESC LIMIT @skipCount,@pageSize";

        public const string GetPostsByCategory =
            @"SELECT p.*,u.UserName AS CreatorName FROM post p LEFT JOIN post_category pc ON pc.PostId = p.Id LEFT JOIN application_user u ON p.CreatorId = u.Id WHERE pc.CategoryId = @CategoryId AND p.IsDeleted = 0 ORDER BY p.CreationTime DESC LIMIT @skipCount,@pageSize";

        public const string GetManagePosts = "SELECT * FROM post WHERE IsDeleted = @IsDeleted AND CreatorId = @UserId ORDER BY p.CreationTime DESC";
    }
}
