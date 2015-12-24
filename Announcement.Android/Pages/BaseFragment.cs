using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Content.Res;
using Android.Views.InputMethods;
using Android.App;
using Android.Widget;
using Announcement.Core;
using Android.OS;

namespace Announcement.Android
{
    public class BaseFragment : global::Android.Support.V4.App.Fragment, View.IOnTouchListener
    {
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            if (view != null)
            {
                view.SetOnTouchListener(this);
            }
        }

        public bool OnTouch(View view, MotionEvent e)
        {
            if (!(view is EditText))
            {
                var manager = (InputMethodManager)Application.Context.GetSystemService(global::Android.Content.Context.InputMethodService);

                if (manager.IsActive)
                {
                    manager.HideSoftInputFromWindow(view.WindowToken, 0);
                }

                var focudesView = Activity.Window.CurrentFocus;

                if (focudesView != null)
                {
                    focudesView.ClearFocus();
                }
            }
            
            return false;
        }
    }
}


