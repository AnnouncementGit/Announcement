using System;
using Android.App;
using Android.Views;
using Android.Content;
using Announcement.Android;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Announcement.Android.Controls;
using Android.Graphics;

namespace Announcement.Core
{
    public static class AlertModule
    {
        public static void Show(string title, string message, string okButton, Action okCallback = null)
		{
			Application.SynchronizationContext.Post (ignored => {
				var activity = NavigationManager.CurrentActivity;

				if (activity == null)
					return;

				var customView = activity.LayoutInflater.Inflate (Resource.Layout.simple_alert, null);
				var builder = new AlertDialogEx.Builder (activity);
				builder.SetView (customView);

				var alertSimpleView = builder.Create ();

				alertSimpleView.Show ();

				alertSimpleView.FindViewById<TextView> (Resource.Id.alertTitle).Text = title;

				alertSimpleView.FindViewById<TextView> (Resource.Id.alertText1).Text = message;

				alertSimpleView.FindViewById<Button> (Resource.Id.alertBtnRetry).Visibility = ViewStates.Gone;

				alertSimpleView.FindViewById<Button> (Resource.Id.alertBtnDismiss).Text = okButton;
				alertSimpleView.FindViewById<Button> (Resource.Id.alertBtnDismiss).Click += (sender, e) => {
					alertSimpleView.Dismiss ();
				};
				alertSimpleView.DismissEvent += (sender, e) => {
					if (okCallback != null)
						okCallback.Invoke ();
				};

			}, null);
		}

        public static void Show(string title, string message, string okButton, string cancelButton, Action okCallback = null, Action cancelCallback = null)
		{
			Application.SynchronizationContext.Post (ignored => {
				var activity = NavigationManager.CurrentActivity;

				if (activity == null)
					return;
					
				var customView = activity.LayoutInflater.Inflate (Resource.Layout.simple_alert, null);
				var builder = new AlertDialogEx.Builder (activity);
				builder.SetView (customView);

				var alertWithCancelView = builder.Create ();

				alertWithCancelView.Show ();

				alertWithCancelView.SetCanceledOnTouchOutside(false);

				alertWithCancelView.FindViewById<TextView> (Resource.Id.alertTitle).Text = title;

				alertWithCancelView.FindViewById<TextView> (Resource.Id.alertText1).Text = message;

				alertWithCancelView.FindViewById<Button> (Resource.Id.alertBtnRetry).Text = okButton;
				alertWithCancelView.FindViewById<Button> (Resource.Id.alertBtnRetry).Click += (sender, e) => {
					alertWithCancelView.Dismiss ();

					if (okCallback != null)
						okCallback.Invoke ();
				};

				alertWithCancelView.FindViewById<Button> (Resource.Id.alertBtnDismiss).Text = cancelButton;
				alertWithCancelView.FindViewById<Button> (Resource.Id.alertBtnDismiss).Click += (sender, e) => {
					alertWithCancelView.Dismiss ();

					if (cancelCallback != null)
						cancelCallback.Invoke ();
				};

				alertWithCancelView.DismissEvent += (sender, e) => {
				};

			}, null);
		}

        public static void ShowError(string message, Action okCallback = null, Action cancelCallback = null)
        {
            Show(LocalizationModule.Translate("alert_title_type_error"), message, LocalizationModule.Translate("alert_button_try_again"), LocalizationModule.Translate("alert_button_cancel"), okCallback, cancelCallback);
        }

        public static void ShowInformation(string message, Action okCallback = null)
        {
            Show(LocalizationModule.Translate("alert_title_type_information"), message, LocalizationModule.Translate("alert_button_ok"), okCallback);
        }

        public static void ShowWarning(string message, Action okCallback = null, Action cancelCallback = null)
        {
            Show(LocalizationModule.Translate("alert_title_type_warning"), message, LocalizationModule.Translate("alert_button_retry"), LocalizationModule.Translate("alert_button_cancel"), okCallback, cancelCallback);
        }
    }

    public class AlertDialogEx : AlertDialog
    {
        public AlertDialogEx(Context context)
            : base(context)
        {
            
        }
    }
}

