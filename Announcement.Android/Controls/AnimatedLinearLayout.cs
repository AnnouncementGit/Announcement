using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Content.Res;
using Android.Views.InputMethods;
using Android.App;
using Android.Animation;
using Android.OS;

namespace Announcement.Android.Controls
{
    public class AnimatedLinearLayout : global::Android.Widget.LinearLayout
    {
        public AnimatedLinearLayout(Context context) : base(context)
        {
            Initialize();
        }

        public AnimatedLinearLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public AnimatedLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public AnimatedLinearLayout(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Initialize();
        }

        protected void Initialize()
        {
            if (Build.VERSION.SdkInt > global::Android.OS.BuildVersionCodes.JellyBean) 
            {
                var layoutTransition = new LayoutTransition();

                layoutTransition.EnableTransitionType(LayoutTransitionType.Changing);

                LayoutTransition = layoutTransition;
            } 
        }
    }
}

