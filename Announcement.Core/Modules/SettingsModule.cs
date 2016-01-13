using System;
using Android.App;
using Android.Views;
using Android.Content;
using Announcement.Android;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Announcement.Android.Controls;
using Android.Graphics;

namespace Announcement.Core
{
    public static class SettingsModule
    {
        static SettingsModule()
        {
            settings = Application.Context.GetSharedPreferences(SETTINGS_FILE_NAME, FileCreationMode.Private);
        }

        public static T Receive<T>(string key)
        {
            try
            {
                if (settings != null)
                { 
                    object result = null;

                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.String:
                            result = settings.GetString(key, null);
                            break;

                        case TypeCode.Int32:
                        case TypeCode.Int64:
                            result = settings.GetInt(key, -1);
                            break;
                    }

                    return (T)result;
                }  
            }
            catch (Exception)
            {

            }

            return default(T);
        }

        public static void Save<T>(string key, T value)
        {
            try
            {
                if (settings != null)
                {
                    var editor = settings.Edit();

                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.String:
                            editor.PutString(key, Convert.ToString(value));
                            break;

                        case TypeCode.Int32:
                        case TypeCode.Int64:
                            editor.PutInt(key, Convert.ToInt32(value));
                            break;
                    }

                    editor.Commit();
                }  
            }
            catch (Exception)
            {
                
            }
        }

        public static void Remove(string key)
        {
            try
            {
                if (settings != null)
                {
                    var editor = settings.Edit();

                    editor.Remove(key);

                    editor.Commit();
                }  
            }
            catch (Exception ex)
            {

            }
        }

        private static ISharedPreferences settings;

        private const string SETTINGS_FILE_NAME = "ANNOUNCEMENT";
    }
}

