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
             
        public async void LoginViaFacebook(string token, Action callback)
        {
            await SocialLogin(token, callback);
        }

        public async void LoginViaGooglePlus(string token, Action callback)
        {
            await SocialLogin(token, callback);
        }

        public async void LoginViaVK(string token, Action callback)
        {
            await SocialLogin(token, callback);
        }

        public async void LoginViaLinkedIn(string token, Action callback)
        {
            await SocialLogin(token, callback);
        }

        public async void LoginForAdminStuff(string username, string password, Action callback)
        {
            var viewModel = AdminMainViewModel.Instance;

            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            var result = await Task.Run<Result>(() => viewModel.Initialize());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => LoginForAdminStuff(username, password, callback));
            }
            else
            {
                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }

        protected async Task SocialLogin(string token, Action callback)
        {
            var viewModel = UserMainViewModel.Instance;

            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            await Task.Delay(1000);

            var result = await Task.Run<Result>(() => viewModel.Initialize());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, async () => await SocialLogin(token, callback));
            }
            else
            {
                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }

        private static LoginViewModel instance;
    }
}

