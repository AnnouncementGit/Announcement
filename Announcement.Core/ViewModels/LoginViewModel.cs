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
            await Test(token, callback);
        }

        public async void LoginViaGooglePlus(string token, Action callback)
        {
            await Test(token, callback);
        }

        public async void LoginViaVK(string token, Action callback)
        {
            await Test(token, callback);
        }

        public async void LoginViaLinkedIn(string token, Action callback)
        {
            await Test(token, callback);
        }

        public async void LoginForAdminStuff(string username, string password, Action callback)
        {
            await Test(null, callback);
        }

        protected async Task Test(string token, Action callback)
        {
            var viewModel = AdminMainViewModel.Instance;

            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            await Task.Delay(3000);

            var result = await Task.Run<Result>(() => viewModel.Initialize());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.Show(result, () => LoginViaFacebook(token, callback));
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
        }

        private static LoginViewModel instance;
    }
}

