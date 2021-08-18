namespace Blog.Data
{
    public static class SqlConstants
    {
        public const string GetCategory = @"SELECT * FROM category WHERE Id = @Id AND IsDeleted = 0";

        public const string GetCategoryByName =
            @"SELECT * FROM category WHERE CategoryName = @CategoryName AND IsDeleted = 0";

        public const string GetCategories = @"SELECT * FROM category WHERE IsDeleted = 0";

        public const string GetPost = @"SELECT * FROM post WHERE Id = @Id AND CreatorId = @CreatorId AND IsDeleted = 0";

        public const string GetPostById = @"SELECT * FROM post WHERE Id = @Id AND IsDeleted = 0";

        public const string GetRestorePost = @"SELECT * FROM post WHERE Id = @Id";

        public const string GetPosts =
            @"SELECT p.*,u.UserName AS CreatorName FROM post p LEFT JOIN application_user u ON p.CreatorId = u.Id WHERE p.IsDeleted = 0 ORDER BY p.CreationTime DESC LIMIT @skipCount,@pageSize";

        public const string GetPostsByCategory =
            @"SELECT p.*,u.UserName AS CreatorName FROM post p LEFT JOIN post_category pc ON pc.PostId = p.Id LEFT JOIN application_user u ON p.CreatorId = u.Id WHERE pc.CategoryId = @CategoryId AND p.IsDeleted = 0 ORDER BY p.CreationTime DESC LIMIT @skipCount,@pageSize";

        public const string GetCategoriesByPost =
            @"SELECT c.* FROM post_category pc LEFT JOIN category c ON pc.CategoryId = c.Id WHERE pc.PostId = @PostId";

        public const string GetManagePosts =
            @"SELECT * FROM post WHERE IsDeleted = @IsDeleted AND CreatorId = @UserId ORDER BY p.CreationTime DESC";

        public const string GetPostCountByCategory =
            @"SELECT COUNT(*) FROM `post_category` pc LEFT JOIN `post` p ON pc.PostId =  p.Id WHERE p.IsDeleted = 0 AND pc.CategoryId = @CategoryId";

        public const string GetPostCount = @"SELECT COUNT(*) FROM post WHERE IsDeleted = 0";

        public const string AddCategory =
            @"INSERT INTO category (Id,CategoryName,CreatorId) VALUES (@Id, @CategoryName, @CreatorId)";

        public const string UpdateCategory =
            @"UPDATE category SET CategoryName = @CategoryName, LastModifierId = @LastModifierId, LastModificationTime = @LastModificationTime WHERE Id = @Id";

        public const string DeleteCategory =
            @"UPDATE category SET IsDeleted = @IsDeleted, DeleterId = @DeleterId, DeletionTime = @DeletionTime WHERE Id = @Id";

        public const string DeletePostCategories = @"DELETE FROM post_category WHERE CategoryId = @CategoryId";

        public const string DeletePost = @"DELETE FROM post WHERE Id = @Id";

        public const string UpdatePost =
            @"UPDATE post SET IsDeleted = @IsDeleted, DeleterId = @DeleterId, DeletionTime = @DeletionTime WHERE Id = @Id";

        public const string RestorePost = "UPDATE post SET IsDeleted = 0 WHERE Id = @Id";
    }
}