namespace Blog.Shared
{
    public static class SqlConstants
    {
        public const string GenerateId = "SELECT UUID() AS Id;";

        #region Category

        public const string GetCategoryById = @"SELECT id AS Id,category_Name AS CategoryName FROM category WHERE id = @id";

        public const string GetCategories = @"SELECT id AS Id,category_Name AS CategoryName FROM category";

        public const string CreateCategory = @"INSERT INTO category (category_name) VALUES (@CategoryName)";

        public const string UpdateCategory = @"UPDATE category SET category_name = @CategoryName WHERE id = @id";

        public const string DeleteCategory = @"DELETE FROM category WHERE id = @id";

        public const string GetCategoriesByPost =
            @"SELECT id AS Id,category_name AS CategoryName FROM category WHERE id IN @ids";

        public const string CategoriesCountByIds = @"SELECT COUNT(*) FROM category WHERE id IN @Ids";

        #endregion

        #region Post

        public const string GetOwnPost =
            @"SELECT BIN_TO_UUID(id) AS Id,title AS Title,content_abstract AS ContentAbstract,content AS Content,creation_time AS CreationTime FROM post WHERE id = UUID_TO_BIN(@id) AND creator_id = @UserId";

        public const string GetPostsPage =
            @"SELECT BIN_TO_UUID(p.id) AS Id,p.title AS Title,p.content_abstract AS ContentAbstract,u.username AS UserName,BIN_TO_UUID(p.creator_id) AS CreatorId,p.creation_time AS CreationTime FROM post p LEFT JOIN app_user u ON p.creator_id = u.id WHERE p.isdeleted = 0 ORDER BY p.creation_time DESC LIMIT @skipCount,@pageSize";

        public const string GetPostsPageByCategory =
            @"SELECT BIN_TO_UUID(p.id) AS Id,p.title AS Title,p.content_abstract AS ContentAbstract,u.username AS UserName,BIN_TO_UUID(p.creator_id) AS CreatorId,p.creation_time AS CreationTime FROM post p LEFT JOIN app_user u ON p.creator_id = u.id WHERE @CategoryId MEMBER OF (category_ids->'$') AND p.isdeleted = 0 ORDER BY p.creation_time DESC LIMIT @skipCount,@pageSize";

        public const string GetPostCountByCategory =
            @"SELECT COUNT(*) FROM `post`WHERE isdeleted = 0 AND @CategoryId MEMBER OF (category_ids->'$')";

        public const string GetPostCount = @"SELECT COUNT(*) FROM post WHERE isdeleted = 0";

        public const string GetPreviewPost =
            @"SELECT BIN_TO_UUID(id) AS Id,title AS Title,content_abstract AS ContentAbstract,content AS Content,category_ids AS CategoryIds,creation_time AS CreationTime FROM post WHERE id = UUID_TO_BIN(@Id) AND isdeleted = 0";

        public const string AddPost =
            @"INSERT INTO post (id,title,content,content_abstract,category_ids,creator_id) VALUES (UUID_TO_BIN(@Id,TRUE), @Title, @Content, @ContentAbstract,@CategoryIds, UUID_TO_BIN(@CreatorId))";
        #endregion
    }
}