using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Announcement.Core
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsAdmin { get; set; }
        
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
            if (!EnteredDataValid(username, password))
            {
                return;
            }
			
            var loginResult = await Task.Run<Result<UserCredentials>>(() => SourceManager.Login(username, EncryptorModule.Encrypt(password)));

            BaseViewModel viewModel = null;

            if (loginResult.HasError || !loginResult.IsSuccess)
            {
                IsAdmin = false;

                ProgressModule.End();

                AlertModule.Show(LocalizationModule.Translate("alert_title_authorization"), LocalizationModule.Translate("alert_message_wrong_credentials"), LocalizationModule.Translate("alert_button_ok"));
            }
            else
            {
                if (loginResult.Value != null && loginResult.Value.Role == 1)
                {
                    viewModel = ModeratorMainViewModel.Instance;

                    IsAdmin = false;
                }
                else
                {
                    viewModel = AdminMainViewModel.Instance;

                    IsAdmin = true;
                }

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
        }

        protected async Task SocialLogin(string token, Action callback)
        {
            var viewModel = UserMainViewModel.Instance;

            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            await Task.Delay(2000);

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

		private bool EnteredDataValid(string username, string password)
		{
			if (string.IsNullOrWhiteSpace (username) || username.Length < 5 || !ValidationModule.ValidateUserName(username)) 
			{
				AlertModule.ShowInformation (LocalizationModule.Translate("alert_incorrect_name_or_password"));
				return false;
			}

			if (string.IsNullOrWhiteSpace (password) || password.Length < 5 || !ValidationModule.ValidatePassword(password)) 
			{
				AlertModule.ShowInformation (LocalizationModule.Translate("alert_incorrect_name_or_password"));
				return false;
			}

			return true;
		}

        private static LoginViewModel instance;
    }
}

