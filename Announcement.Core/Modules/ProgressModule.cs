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
using Android.Content;

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
                    {
                        return;
                    }

                    if (progressView == null)
                    {
                        progressView = new ProgressView(activity, borderless ? Resource.Layout.progress_borderless_layout : Resource.Layout.progress_layout);
                    }

                    progressView.Title = message;

                    if(!progressView.IsShowing)
                    {
                        progressView.Show(activity.Window.DecorView);
                    }

                }, null);
        }


        public static ProgressAwaiter MessageWithAwaiter(string message, bool borderless = false)
        {
            var awaiter = new ProgressAwaiter();
            
            Message(message, borderless);

            return awaiter;
        }

        public static void End()
        {
            Application.SynchronizationContext.Post(ignored =>
                {
                    var activity = NavigationManager.CurrentActivity;

                    if (activity == null)
                    {
                        return;
                    }
            
                    if (progressView != null)
                    {
                        progressView.Dismiss();

                        progressView.Dispose();

                        progressView = null;
                    }

                }, null);
        }

        private static ProgressView progressView;
    }

    public class ProgressAwaiter
    {
        public ProgressAwaiter()
        {
            startTime = DateTime.Now.TimeOfDay;
        }
                
        public Task OperationTime()
        {
            var endTime = DateTime.Now.TimeOfDay;
            
            var elapsed = endTime.TotalMilliseconds - startTime.TotalMilliseconds;

            var timeToWait = (int)(MINIMUN_TIME - elapsed);

            if (timeToWait > 0)
            {
                return Task.Delay(timeToWait);
            }

            return new Task(null);
        }

        private TimeSpan startTime;

        private const int MINIMUN_TIME = 1000;
    }

    public class ProgressView : PopupWindow
    {
        public String Title 
        {
            get
            {
                if (title != null)
                {
                    return title.Text;
                }
                return string.Empty;
            }
            set
            {
                if (title != null)
                {
                    title.Text = value;
                }
            }
        }
        
        public ProgressView(Context context, int id) : base(context)
        {
            var inflater = LayoutInflater.From(context);

            content = inflater.Inflate(id, null);

            title = content.FindViewById<TextView>(Resource.Id.progressTitle);

            base.Width = WindowManagerLayoutParams.MatchParent;

            base.Height = WindowManagerLayoutParams.MatchParent;

            base.ContentView = content;

            SetBackgroundDrawable(null);
        }

        public void Show(View target)
        {
            base.ShowAtLocation(target, GravityFlags.Center, 0, 0);
        }

        private TextView title;

        private View content;
    }
}

