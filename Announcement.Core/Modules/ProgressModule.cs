using System;
using Android.App;
using Android.Views;
using Announcement.Android;
using System.Threading.Tasks;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Collections.Generic;
using Android.Content.Res;

namespace Announcement.Core
{
    public class ProgressModule
    {
        public static void Message(string message, bool borderless = false)
        {
            Application.SynchronizationContext.Post(ignored =>
                {
                    var activity = NavigationManager.CurrentActivity;

                    if (activity == null)
                        return;

                    if (progressView == null)
                    {
                        View view = null;

                        ProgressDialog.Builder builder = null;

                        if (borderless)
                        {
                            view = activity.LayoutInflater.Inflate(Resource.Layout.progress_borderless_layout, null);

                            builder = new ProgressDialog.Builder(activity, Resource.Style.progress_dialog_borderless_theme);
                        }
                        else
                        {  
                            view = activity.LayoutInflater.Inflate(Resource.Layout.progress_layout, null);

                            builder = new ProgressDialog.Builder(activity, Resource.Style.progress_dialog_theme);                            
                        }
                            
                        builder.SetView(view);

                        progressView = builder.Create();

                        progressView.SetCancelable(false);

                        progressView.SetCanceledOnTouchOutside(false);

                        if (!progressView.IsShowing)
                        {
                            progressView.Show();
                        }
                    }

                    progressView.FindViewById<TextView>(Resource.Id.progressTitle).Text = message;

                    if (!progressView.IsShowing)
                    {
                        progressView.Show();
                    }

                }, null);
        }

        public static void End()
        {
            Application.SynchronizationContext.Post(ignored =>
                {
                    var activity = NavigationManager.CurrentActivity;

                    if (activity == null)
                        return;
            
                    if (progressView != null)
                    {
                        progressView.Dismiss();
                    }

                }, null);
        }

        private static AlertDialog progressView;
    }
}

