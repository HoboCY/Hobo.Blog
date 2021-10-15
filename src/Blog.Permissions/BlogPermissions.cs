namespace Blog.Permissions
{
    public static class BlogPermissions
    {
        private const string Post = "Blog.Posts";

        private const string Category = "Blog.Categories";

        private const string User = "Blog.Users";

        private const string Role = "Blog.Roles";

        private const string Menu = "Blog.Menus";

        public static class Posts
        {
            public const string Get = Post + ".Get";

            public const string GetList = Post + ".GetList";

            public const string Create = Post + ".Create";

            public const string Update = Post + ".Update";

            public const string Delete = Post + ".Delete";

            public const string Recycle = Post + ".Recycle";

            public const string Restore = Post + ".Restore";
        }

        public static class Categories
        {
            public const string Create = Category + ".Create";

            public const string Update = Category + ".Update";

            public const string Delete = Category + ".Delete";
        }

        public static class Users
        {
            public const string Get = User + ".Get";

            public const string GetRoles = User + ".GetRoles";

            public const string SetRoles = User + ".SetRoles";

            public const string Confirm = User + ".Confirm";
        }

        public static class Roles
        {
            public const string Get = Role + ".Get";

            public const string GetAllPermissions = Role + ".GetAllPermissions";

            public const string GetRolePermissions = Role + ".GetRolePermissions";

            public const string Create = Role + ".Create";

            public const string Update = Role + ".Update";

            public const string GrantPermissions = Role + ".Permissions.Grant";
        }

        public static class Menus
        {
            public const string Get = Menu + ".Get";

            public const string Create = Menu + ".Create";

            public const string Delete = Menu + ".Delete";

            public const string Update = Menu + ".Update";
        }
    }
}
