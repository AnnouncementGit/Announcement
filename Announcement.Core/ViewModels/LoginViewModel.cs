using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class LoginViewModel : BaseViewModel
    {
        public static LoginViewModel Instance
        {
            get
            {
                return instance ?? (instance = new LoginViewModel());
            }
        }
            
        public LoginViewModel()
        {
            
        }

        public override InitializationStatus Initialize()
        {
            return base.Initialize();
        }

        public async void LoginViaFacebook(string token, Action callback)
        {
            Test(token, callback);
        }

        public async void LoginViaGooglePlus(string token, Action callback)
        {
            Test(token, callback);
        }

        public async void LoginViaVK(string token, Action callback)
        {
            Test(token, callback);
        }

        public async void LoginViaLinkedIn(string token, Action callback)
        {
            Test(token, callback);
        }

        public async void LoginForAdminStuff(string username, string password, Action callback)
        {
            Test(null, callback);
        }

        protected async void Test(string token, Action callback)
        {
            //
            //TODO
            //
            var viewModel = AdminMainViewModel.Instance;

            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            await Task.Delay(3000);

            var status = await Task.Run<InitializationStatus>(() => viewModel.Initialize());

            ProgressModule.End();

            if (status.IsSuccess)
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
            else
            {
                AlertModule.Show(AlertModule.INFORMATION, status.Message, LocalizationModule.Translate("alert_try_again"), () => LoginViaFacebook(token, callback));
            }
        }

        private static LoginViewModel instance;
    }
}

