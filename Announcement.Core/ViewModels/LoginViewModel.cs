using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

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
             
        public async void LoginViaFacebook(string userId, string token, Action callback)
        {
            await SocialLogin(userId, token, callback);
        }

        public async void LoginViaGooglePlus(string userId, string token, Action callback)
        {
            await SocialLogin(userId, token, callback);
        }

        public async void LoginViaVK(string userId, string token, Action callback)
        {
            await SocialLogin(userId, token, callback);
        }

        public async void LoginViaLinkedIn(string userId, string token, Action callback)
        {
            await SocialLogin(userId, token, callback);
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
                ProgressModule.End();

                if(loginResult.ErrorCode == ErrorCodes.UserWrongCredentials)
                {
                    AlertModule.Show(LocalizationModule.Translate("alert_title_authorization"), LocalizationModule.Translate("alert_message_wrong_credentials"), LocalizationModule.Translate("alert_button_ok"));
                }
                else
                {
                    AlertModule.ShowError(loginResult.Message, () => LoginForAdminStuff(username, password, callback));
                }
            }
            else
            {
                UserInfo = loginResult.Value;
                
                if (UserInfo != null)
                {
                    switch(UserInfo.Role)
                    {
                        case UserRoles.Admin:
                            viewModel = AdminMainViewModel.Instance; 
                            break;
                        default:
                            viewModel = ModeratorMainViewModel.Instance; 
                            break;
                    }
                }

                var result = await Task.Run<Result>(() => viewModel.Initialize());

                ProgressModule.End();

                if (result.HasError)
                {
                    AlertModule.ShowError(result.Message, () => LoginForAdminStuff(username, password, callback));
                }
                else
                {
                    SettingsModule.Save(Constants.USER_ID, UserInfo.Username);

                    SettingsModule.Save(Constants.USER_ROLE, UserInfo.Role);

                    SettingsModule.Save(Constants.USER_ACCESS_TOKEN, UserInfo.AccessToken);
                    
                    if (callback != null)
                    {                   
                        DispatcherModule.Invoke(callback);
                    }
                }
            }
        }

        protected async Task SocialLogin(string userId, string token, Action callback)
        {
            var viewModel = UserMainViewModel.Instance;
            
            var loginResult = await Task.Run<Result<UserCredentials>>(() => SourceManager.LoginViaSocial(userId, EncryptorModule.Encrypt("dsdsdsds")));

            if (loginResult.HasError || !loginResult.IsSuccess)
            {
                ProgressModule.End();

                if(loginResult.ErrorCode == ErrorCodes.UserWrongCredentials)
                {
                    AlertModule.Show(LocalizationModule.Translate("alert_title_authorization"), LocalizationModule.Translate("alert_message_wrong_credentials"), LocalizationModule.Translate("alert_button_ok"));
                }
                else
                {
                    AlertModule.ShowError(loginResult.Message, () => SocialLogin(userId, token, callback));
                }
            }
            else
            {
                UserInfo = loginResult.Value;

                var result = await Task.Run<Result>(() => viewModel.Initialize());

                ProgressModule.End();

                if (result.HasError)
                {
                    AlertModule.ShowError(result.Message, () => SocialLogin(userId, token, callback));
                }
                else
                {
                    SettingsModule.Save(Constants.USER_ID, UserInfo.Username);

                    SettingsModule.Save(Constants.USER_ROLE, UserInfo.Role);

                    SettingsModule.Save(Constants.USER_ACCESS_TOKEN, UserInfo.AccessToken);
                    
                    if (callback != null)
                    {                   
                        DispatcherModule.Invoke(callback);
                    }
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

