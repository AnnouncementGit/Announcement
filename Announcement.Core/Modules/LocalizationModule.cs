using System;

namespace Announcement.Android
{
    public static class LocalizationModule
    {
        public static string Translate(string key)
        {
            string result = "[null]";

            if(!string.IsNullOrEmpty(key) && !string.IsNullOrWhiteSpace(key))
            {
                result = Language.ResourceManager.GetString(key);
            }

            return result;
        }
    }
}

