using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Extensions
{
    public static class EncryptExtensions
    {
        public static string Encrypt(this string input)
        {
            using var md5 = MD5.Create();
            var data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var encryptString = BitConverter.ToString(data);
            return encryptString.Replace("-", "");
        }
    }
}
