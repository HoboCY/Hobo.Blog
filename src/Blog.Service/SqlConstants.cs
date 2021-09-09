namespace Blog.Service
{
    public static class SqlConstants
    {
        #region Category

        public const string GetCategoryById = @"SELECT id AS Id,category_Name AS CategoryName FROM category WHERE id = @id";

        public const string GetCategories = @"SELECT id AS Id,category_Name AS CategoryName FROM category";

        public const string CreateCategory = @"INSERT INTO category (category_name) VALUES (@CategoryName)";

        public const string UpdateCategory = @"UPDATE category SET category_name = @CategoryName WHERE id = @id";

        public const string DeleteCategory = @"DELETE FROM category WHERE id = @id";

        public const string GetCategoriesByPost =
            @"SELECT c.id AS Id,c.category_name AS CategoryName FROM post_category pc LEFT JOIN category c ON pc.category_id = c.Id WHERE pc.post_id = @PostId";

        #endregion

        #region PostCategory
        public const string DeletePostCategoriesByCategory = @"DELETE FROM post_category WHERE category_id = @id";


        #endregion


        public const string GetCategoryByName =
            @"SELECT * FROM category WHERE CategoryName = @CategoryName AND IsDeleted = 0";


        #region Post

        public const string GetOwnPost =
            @"SELECT BIN_TO_UUID(id) AS Id,title AS Title,content_abstract AS ContentAbstract,content AS Content,creation_time AS CreationTime FROM post WHERE id = UUID_TO_BIN(@id) AND creator_id = @UserId";

        public const string GetPostsPage =
            @"SELECT BIN_TO_UUID(p.id) AS Id,p.title AS Title,p.content_abstract AS ContentAbstract,u.username AS UserName,BIN_TO_UUID(p.creator_id) AS CreatorId,p.creation_time AS CreationTime FROM post p LEFT JOIN app_user u ON p.creator_id = u.id WHERE p.isdeleted = 0 ORDER BY p.creation_time DESC LIMIT @skipCount,@pageSize";

        public const string GetPostsPageByCategory =
            @"SELECT BIN_TO_UUID(p.id) AS Id,p.title AS Title,p.content_abstract AS ContentAbstract,u.username AS UserName,BIN_TO_UUID(p.creator_id) AS CreatorId,p.creation_time AS CreationTime FROM post p LEFT JOIN post_category pc ON pc.post_id = p.id LEFT JOIN app_user u ON p.creator_id = u.id WHERE pc.category_id = @CategoryId AND p.isdeleted = 0 ORDER BY p.creation_time DESC LIMIT @skipCount,@pageSize";

        public const string GetPostCountByCategory =
            @"SELECT COUNT(*) FROM `post_category` pc LEFT JOIN `post` p ON pc.post_id =  p.id WHERE p.isdeleted = 0 AND pc.category_id = @CategoryId";

        public const string GetPostCount = @"SELECT COUNT(*) FROM post WHERE isdeleted = 0";

        public const string GetPreviewPost =
            @"SELECT BIN_TO_UUID(p.id) AS Id,p.title AS Title,p.content_abstract AS ContentAbstract,p.Content AS Content,p.creation_time AS CreationTime FROM post WHERE id = @Id AND creator_id = @UserId AND isdeleted = 0";

        public const string AddPost =
            @"INSERT INTO post (UUID_TO_BIN(UUID(),TRUE),title,content,content_abstract,creator_id) VALUES (@Id, @Title, @Content, @ContentAbstract, @CreatorId)";

        public const string AddPostCategory = @"INSERT INTO post_category (category_id, post_id) VALUES (@CategoryId, UUID_TO_BIN(@PostId,TRUE))";
        #endregion






        public const string GetPostToEdit = @"SELECT * FROM post WHERE Id = @Id AND CreatorId = @CreatorId AND IsDeleted = 0";

        

        

        public const string UpdatePost =
            @"UPDATE post SET Title = @Title,Content = @Content,ContentAbstract = @ContentAbstract,LastModificationTime = @LastModificationTime WHERE Id = @Id";

        public const string DeletePostCategoryByPost = @"DELETE FROM post_category WHERE PostId = @PostId";
    }
}