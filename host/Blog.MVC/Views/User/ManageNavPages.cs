using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Views.User
{
    public static class ManageNavPages
    {
        public static string Profile => "Profile";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string DeletePersonalData => "DeletePersonalData";

        public static string PersonalData => "PersonalData";

        public static string CreateOrEditPost => "CreateOrEditPost";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Profile);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string CreatePostNavClass(ViewContext viewContext) => PageNavClass(viewContext, CreateOrEditPost);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}