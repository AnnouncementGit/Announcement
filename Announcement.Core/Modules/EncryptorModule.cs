using System;
using Android.App;
using Android.Views;
using Android.Content;
using Announcement.Android;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Announcement.Android.Controls;
using Android.Graphics;
using System.Security.Cryptography;
using System.Text;

namespace Announcement.Core
{
    public static class EncryptorModule
    {
        public static string Encrypt(string value)
        {
            var password = Encoding.UTF8.GetBytes(value);
            
            var hmacMD5 = new HMACMD5(Encoding.UTF8.GetBytes(ENCRYPTION_SALT));

            byte[] result = hmacMD5.ComputeHash(password);

            var builder = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                builder.Append(result[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private const string ENCRYPTION_SALT = "x-#1d@p0*";
    }
}

