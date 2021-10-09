namespace Blog.Permissions
{
    public static class BlogPermissions
    {
        public const string Post = "Blog.Posts";

        public static class Posts
        {
            public const string Create = Post + ".Create";

            public const string Update = Post + ".Update";

            public const string Delete = Post + ".Delete";

            public const string Recycle = Post + ".Recycle";

            public const string Restore = Post + ".Restore";
        }
    }
}
