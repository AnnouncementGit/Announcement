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
        public static void Message(string message)
        {
            Application.SynchronizationContext.Post(ignored =>
                {
                    var activity = NavigationManager.CurrentActivity;

                    if (activity == null)
                        return;

                    if (progressView == null)
                    {
                        var customView = activity.LayoutInflater.Inflate(Resource.Layout.simple_progress, null);

                        ProgressDialog.Builder builder = new ProgressDialog.Builder(activity, Resource.Style.progress_dialog_theme);
                     
                        builder.SetView(customView);

                        progressView = builder.Create();

                        progressView.SetView(customView, 0,0,0,0);

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

