using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Blog.MVC.Models.User
{
    public static class ManageNavPages
    {
        public static string Profile => "Profile";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string DeletePersonalData => "DeletePersonalData";

        public static string PersonalData => "PersonalData";

        public static string CreateOrEditPost => "CreateOrEditPost";

        public static string ManagePost => "ManagePost";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string CreatePostNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateOrEditPost);

        public static string ManagePostNavClass(ViewContext viewContext) => PageNavClass(viewContext, ManagePost);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}