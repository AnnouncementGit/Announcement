using System;
using Android.App;
using Android.Views;
using Announcement.Android;
using System.Threading.Tasks;

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
                        progressView = new ProgressDialog(activity);

                        progressView.SetProgressStyle(ProgressDialogStyle.Spinner);

                        progressView.SetTitle(LocalizationModule.Translate("progress_title"));

                        progressView.SetCancelable(false);

                        progressView.SetCanceledOnTouchOutside(false);

                        progressView.Indeterminate = true;
                    }

                    progressView.SetMessage(message);

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
            
        private static ProgressDialog progressView;
	}
}

