using System;
using Android.App;
using Android.Views;
using Android.Content;
using Announcement.Android;
using System.Threading.Tasks;

namespace Announcement.Core
{
    public static class AlertModule
    {
        public static void Show(string title, string message, string okButton, Action okCallback = null)
        {
            Application.SynchronizationContext.Post(ignored =>
                {
                    var activity = NavigationManager.CurrentActivity;

                    if (activity == null)
                        return;

                    if (alertSimpleView == null)
                    {
                        alertSimpleView = new AlertDialogEx(activity);
                    }

                    alertSimpleView.SetTitle(title);

                    alertSimpleView.SetMessage(message);

                    alertSimpleView.SetButton(okButton, handler: (s, e) =>
                        { 
                            if (okCallback != null)
                            {
                                okCallback.Invoke();
                            }
                        });

                    alertSimpleView.Show();

                }, null);
        }

        public static void Show(string title, string message, string okButton, string cancelButton, Action okCallback = null, Action cancelCallback = null)
        {
            Application.SynchronizationContext.Post(ignored =>
                {
                    var activity = NavigationManager.CurrentActivity;

                    if (activity == null)
                        return;

                    if (alertWithCancelView == null)
                    {
                        alertWithCancelView = new AlertDialogEx(activity);
                    }

                    alertWithCancelView.SetTitle(title);

                    alertWithCancelView.SetMessage(message);

                    alertWithCancelView.SetButton(okButton, handler: (s, e) =>
                        { 
                            if (okCallback != null)
                            {
                                okCallback.Invoke();
                            }
                        });

                    alertWithCancelView.SetButton2(cancelButton, handler: (s, e) =>
                        { 
                            if (cancelCallback != null)
                            {
                                cancelCallback.Invoke();
                            }
                        });

                    alertWithCancelView.Show();

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
            Show(LocalizationModule.Translate("alert_title_type_warning"), message, LocalizationModule.Translate("alert_button_try_again"), LocalizationModule.Translate("alert_button_cancel"), okCallback, cancelCallback);
        }
            

        private static AlertDialog alertSimpleView;

        private static AlertDialog alertWithCancelView;
    }

    public class AlertDialogEx : AlertDialog
    {
        public AlertDialogEx(Context context)
            : base(context)
        {
            
        }
    }
}

