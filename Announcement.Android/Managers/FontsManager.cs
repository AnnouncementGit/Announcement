using Android.App;
using Android.Graphics;

namespace Announcement.Android
{
	public static class FontsManager
	{
        public static Typeface times_new_roman;

        public static Typeface times_new_roman_bold;

        public static void Load()
        {
            times_new_roman = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/times_new_roman.ttf");

            times_new_roman_bold = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/times_new_roman_bold.ttf");
        }

        public static Typeface LoadByName(string fontName)
        {
            switch (fontName)
            {
                case "times_new_roman":
                    return times_new_roman;

                case "times_new_roman_bold":
                    return times_new_roman_bold;
            }

            return times_new_roman;
        }
	}
}

