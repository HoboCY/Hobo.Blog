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

        public const string GetPost = @"SELECT BIN_TO_UUID(id) AS Id,title AS Title,content AS Content,category_ids AS CategoryIds,BIN_TO_UUID(creator_id) AS CreatorId FROM post WHERE id = UUID_TO_BIN(@id) AND IsDeleted = 0";

        public const string GetPostsPage =
            @"SELECT BIN_TO_UUID(p.id) AS Id,p.title AS Title,p.content_abstract AS ContentAbstract,u.username AS UserName,BIN_TO_UUID(p.creator_id) AS CreatorId,p.creation_time AS CreationTime FROM post p LEFT JOIN app_user u ON p.creator_id = u.id WHERE p.isdeleted = 0 ORDER BY p.creation_time DESC LIMIT @skipCount,@pageSize";

        public const string GetOwnPostsPage =
            @"SELECT BIN_TO_UUID(p.id) AS Id,p.title AS Title,p.content_abstract AS ContentAbstract,p.creation_time AS CreationTime,p.last_modify_time AS LastModifyTime,JSON_ARRAYAGG(c.category_name) AS CategoryNames FROM post p JOIN JSON_TABLE(p.category_ids,'$[*]' COLUMNS (CategoryId INT PATH '$')) AS categories JOIN category c ON categories.CategoryId = c.id WHERE p.isdeleted = @IsDeleted AND p.creator_id = UUID_TO_BIN(@UserId) GROUP BY p.id ORDER BY p.creation_time DESC LIMIT @skipCount,@pageSize";

        public const string GetOwnPostsTotalCount =
            @"SELECT COUNT(id) FROM post WHERE isdeleted = @IsDeleted AND creator_id = UUID_TO_BIN(@UserId)";

        public const string GetPostsPageByCategory =
            @"SELECT BIN_TO_UUID(p.id) AS Id,p.title AS Title,p.content_abstract AS ContentAbstract,u.username AS UserName,BIN_TO_UUID(p.creator_id) AS CreatorId,p.creation_time AS CreationTime FROM post p LEFT JOIN app_user u ON p.creator_id = u.id WHERE @CategoryId MEMBER OF (category_ids->'$') AND p.isdeleted = 0 ORDER BY p.creation_time DESC LIMIT @skipCount,@pageSize";

        public const string GetPostCountByCategory =
            @"SELECT COUNT(*) FROM `post`WHERE isdeleted = 0 AND @CategoryId MEMBER OF (category_ids->'$')";

        public const string GetPostCount = @"SELECT COUNT(*) FROM post WHERE isdeleted = 0";

        public const string GetPreviewPost =
            @"SELECT BIN_TO_UUID(id) AS Id,title AS Title,content_abstract AS ContentAbstract,content AS Content,category_ids AS CategoryIds,creation_time AS CreationTime,last_modify_time AS LastModifyTime FROM post WHERE id = UUID_TO_BIN(@Id) AND isdeleted = 0";

        public const string AddPost =
            @"INSERT INTO post (id,title,content,content_abstract,category_ids,creator_id) VALUES (UUID_TO_BIN(@Id,TRUE), @Title, @Content, @ContentAbstract,@CategoryIds, UUID_TO_BIN(@CreatorId))";

        public const string UpdatePost =
            @"UPDATE post SET title = @Title,content = @Content,content_abstract = @ContentAbstract,category_ids = @CategoryIds WHERE id = UUID_TO_BIN(@id)";

        public const string RecycleOrRestorePost = @"UPDATE post SET isdeleted = @IsDeleted WHERE id = UUID_TO_BIN(@Id)";

        public const string DeletePost = @"DELETE FROM post WHERE id = UUID_TO_BIN(@Id)";

        #endregion

        #region User

        public const string Login =
            @"SELECT BIN_TO_UUID(id) AS Id,email AS Email,email_confirmed AS EmailConfirmed,password AS Password FROM `app_user` WHERE email = @Email";

        public const string GetUser = "SELECT BIN_TO_UUID(id) AS Id,username AS Username,email AS Email,email_confirmed AS EmailConfirmed,creation_time AS CreationTime,last_modify_time AS LastModifyTime FROM `app_user` WHERE id = UUID_TO_BIN(@Id)";

        public const string GetUsersPage = "SELECT BIN_TO_UUID(id) AS Id,username AS Username,email AS Email,email_confirmed AS EmailConfirmed,creation_time AS CreationTime,last_modify_time AS LastModifyTime FROM `app_user` WHERE id != UUID_TO_BIN(@UserId) ORDER BY creation_time DESC LIMIT @skipCount,@pageSize";

        public const string GetUsersTotalCount =
            @"SELECT COUNT(id) FROM `app_user` WHERE id != UUID_TO_BIN(@UserId)";

        public const string ConfirmUser =
            @"UPDATE `app_user` SET email_confirmed = @Confirmed WHERE id = UUID_TO_BIN(@UserId)";

        public const string GetUserRoles = @"SELECT r.id AS Id,r.role_name AS RoleName FROM user_role ur LEFT JOIN role r ON r.id = ur.role_id WHERE ur.user_id = UUID_TO_BIN(@UserId)";

        public const string CheckRolePermissions = @"SELECT 1 FROM role_permission p LEFT JOIN role r ON p.role_id = r.id WHERE r.role_name IN @Roles AND JSON_CONTAINS(permissions,'""{0}""') LIMIT 1";

        #endregion

        #region Role

        public const string GetRoles = @"SELECT id AS Id,role_name AS RoleName,creation_time AS CreationTime FROM role";

        public const string CreateRole = @"INSERT INTO role (role_name) VALUES (@Role)";

        public const string UpdateRole = @"UPDATE role SET role_name = @Role WHERE id = @RoleId";

        public const string CheckRolePermissionExist = @"SELECT 1 FROM role_permission WHERE role_id = @RoleId LIMIT 1";

        public const string CreateRolePermissions = @"INSERT INTO role_permission (role_id,permissions) VALUES (@RoleId,@Permissions)";

        public const string UpdateRolePermissions =
            @"UPDATE role_permission SET permissions = @Permissions WHERE role_id = @RoleId";

        public const string GetRolePermissions = @"SELECT Permissions.Permission FROM role_permission p JOIN JSON_TABLE(p.permissions,'$[*]' COLUMNS (Permission TEXT PATH '$')) AS Permissions WHERE p.role_id = @RoleId";

        public const string DeleteUserRoles = @"DELETE FROM user_role WHERE user_id = UUID_TO_BIN(@UserId)";

        public const string SetUserRoles = @"INSERT INTO user_role (user_id,role_id) VALUES (UUID_TO_BIN(@UserId),@RoleId)";

        public const string GetRoleMenus =
            @"SELECT m.id AS Id,m.parent_id AS ParentId,m.url AS Url,m.level AS Level,m.text AS Text FROM role_menu ru LEFT JOIN menu m ON ru.menu_id = m.id WHERE ru.role_id IN @RoleIds";

        public const string DeleteRoleMenus = @"DELETE FROM role_menu WHERE role_id = @RoleId";

        public const string CreateRoleMenus = @"INSERT INTO role_menu (role_id,menu_id) VALUES (@RoleId,@MenuId)";
        #endregion

        #region Menu

        public const string GetAllMenus =
            @"SELECT id AS Id,parent_id AS ParentId,url AS Url,level AS Level,text AS Text FROM menu";

        public const string CreateMenu =
            @"INSERT INTO menu (parent_id,url,text,level) VALUES (@ParentId,@Url,@Text,@Level)";

        public const string GetLastId = @"SELECT LAST_INSERT_ID()";

        public const string GetMenu = @"SELECT id AS Id,parent_id AS ParentId,url AS Url,level AS Level,text AS Text FROM menu WHERE id = @Id";

        public const string DeleteMenu = @"DELETE FROM menu WHERE id = @MenuId";

        public const string DeleteChildrenMenus = @"DELETE FROM menu WHERE parent_id = @ParentId";

        public const string UpdateMenu = @"UPDATE menu SET text = @Text,url = @Url WHERE id = @MenuId";

        #endregion

    }
}