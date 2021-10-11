using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Permissions
{
    public static class BlogPermissionsExtensions
    {
        public static List<string> GetPermissions()
        {
            var type = typeof(BlogPermissions);
            var nestedTypes = type.GetNestedTypes();
            return nestedTypes.SelectMany(t => t.GetFields()).Select(p => p.GetRawConstantValue().ToString()).ToList();
        }
    }
}
