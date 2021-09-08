namespace Blog.Service
{
    public static class SqlConstants
    {
        #region Category

        public const string GetCategoryById = @"SELECT * FROM category WHERE Id = @Id";

        public const string GetCategories = @"SELECT * FROM category";

        public const string CreateCategory = @"INSERT INTO category (CategoryName) VALUES (@CategoryName)";

        public const string UpdateCategory = @"UPDATE category SET CategoryName = @CategoryName WHERE Id = @Id";

        #endregion


        public const string GetCategoryByName =
            @"SELECT * FROM category WHERE CategoryName = @CategoryName AND IsDeleted = 0";



        public const string GetOwnPost = @"SELECT * FROM post WHERE Id = @Id AND CreatorId = @CreatorId AND IsDeleted = 0";

        public const string GetPostToDelete = @"SELECT * FROM post WHERE Id = @Id AND CreatorId = @CreatorId AND IsDeleted = 0";

        public const string GetRestorePost = @"SELECT * FROM post WHERE Id = @Id";

        public const string GetPostsPage =
            @"SELECT p.*,u.UserName AS CreatorName FROM post p LEFT JOIN application_user u ON p.CreatorId = u.Id WHERE p.IsDeleted = 0 ORDER BY p.CreationTime DESC LIMIT @skipCount,@pageSize";

        public const string GetPostsPageByCategory =
            @"SELECT p.*,u.UserName AS CreatorName FROM post p LEFT JOIN post_category pc ON pc.PostId = p.Id LEFT JOIN application_user u ON p.CreatorId = u.Id WHERE pc.CategoryId = @CategoryId AND p.IsDeleted = 0 ORDER BY p.CreationTime DESC LIMIT @skipCount,@pageSize";

        public const string GetCategoriesByPost =
            @"SELECT c.* FROM post_category pc LEFT JOIN category c ON pc.CategoryId = c.Id WHERE pc.PostId = @PostId";

        public const string GetManagePosts =
            @"SELECT * FROM post WHERE IsDeleted = @IsDeleted AND CreatorId = @UserId ORDER BY p.CreationTime DESC";

        public const string GetPostCountByCategory =
            @"SELECT COUNT(*) FROM `post_category` pc LEFT JOIN `post` p ON pc.PostId =  p.Id WHERE p.IsDeleted = 0 AND pc.CategoryId = @CategoryId";

        public const string GetPostCount = @"SELECT COUNT(*) FROM post WHERE IsDeleted = 0";

        public const string GetPostToEdit = @"SELECT * FROM post WHERE Id = @Id AND CreatorId = @CreatorId AND IsDeleted = 0";

        public const string AddCategory =
            @"INSERT INTO category (Id,CategoryName,CreatorId) VALUES (@Id, @CategoryName, @CreatorId)";

        public const string DeleteCategory =
            @"UPDATE category SET IsDeleted = @IsDeleted, DeleterId = @DeleterId, DeletionTime = @DeletionTime WHERE Id = @Id";

        public const string DeletePostCategoriesByCategory = @"DELETE FROM post_category WHERE CategoryId = @CategoryId";

        public const string DeletePost = @"DELETE FROM post WHERE Id = @Id";

        public const string SoftDeletePost =
            @"UPDATE post SET IsDeleted = @IsDeleted, DeleterId = @DeleterId, DeletionTime = @DeletionTime WHERE Id = @Id";

        public const string RestorePost = "UPDATE post SET IsDeleted = 0 WHERE Id = @Id";

        public const string AddPost = @"INSERT INTO post (Id,Title,Content,ContentAbstract,CreatorId) VALUES (@Id, @Title, @Content, @ContentAbstract, @CreatorId)";

        public const string AddPostCategory = @"INSERT INTO post_category (CategoryId, PostId) VALUES (@CategoryId, @PostId)";

        public const string UpdatePost =
            @"UPDATE post SET Title = @Title,Content = @Content,ContentAbstract = @ContentAbstract,LastModificationTime = @LastModificationTime WHERE Id = @Id";

        public const string DeletePostCategoryByPost = @"DELETE FROM post_category WHERE PostId = @PostId";
    }
}