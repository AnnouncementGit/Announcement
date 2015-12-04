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
        public static void Show(Result result, Action okCallback = null)
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

                    var okButton = "[none]";

                    var title = "[none]";

                    switch (result.Type)
                    {
                        case ResultType.Error:
                            title = LocalizationModule.Translate("alert_title_type_error");
                            okButton = LocalizationModule.Translate("alert_button_try_again");
                            break;
                        case ResultType.Warning:
                            title = LocalizationModule.Translate("alert_title_type_warning");
                            okButton = LocalizationModule.Translate("alert_button_ok");
                            break;
                        case ResultType.Information:
                            title = LocalizationModule.Translate("alert_title_type_information");
                            okButton = LocalizationModule.Translate("alert_button_ok");
                            break;
                        case ResultType.Success:
                            title = LocalizationModule.Translate("alert_title_type_success");
                            okButton = LocalizationModule.Translate("alert_button_ok");
                            break;
                    }

                    alertSimpleView.SetTitle(title);

                    alertSimpleView.SetMessage(result.Message);

                    alertSimpleView.SetButton(okButton, handler: (s, e) =>
                        { 
                            if (result.Type == ResultType.Error && okCallback != null)
                            {
                                okCallback.Invoke();
                            }
                        });

                    alertSimpleView.Show();

                }, null);
        }

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

