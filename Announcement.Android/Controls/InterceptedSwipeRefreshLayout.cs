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
using Android.Support.V4.Widget;

namespace Announcement.Android.Controls
{
    public class InterceptedSwipeRefreshLayout : SwipeRefreshLayout 
    {
        public bool IsIntercept { get; set; }

        public InterceptedSwipeRefreshLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public InterceptedSwipeRefreshLayout(Context context): base(context)
        {
            Initialize();
        }
            
        public InterceptedSwipeRefreshLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }
            
        protected void Initialize()
        {

        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (IsIntercept)
            {
                return false;
            }
            else
            {
                return  base.OnInterceptTouchEvent(ev); 
            }
        }
    }
}

