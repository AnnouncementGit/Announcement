using Android.App;
using Android.Graphics;
using Android.Util;

namespace Announcement.Android
{
    public static class ValueConverterManager
    {
        public static int PxToDp(int value)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Px, value, Application.Context.Resources.DisplayMetrics);
        }

        public static int DpToPx(int value)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, value, Application.Context.Resources.DisplayMetrics);
        }

        public static int SpToPx(int value)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Sp, value, Application.Context.Resources.DisplayMetrics);
        }

        public static int PxToSp(int value)
        {
            return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, value, Application.Context.Resources.DisplayMetrics);
        }
    }
}

