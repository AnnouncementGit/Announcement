using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace Announcement.Core
{
    public class MainViewModel : BaseViewModel
    {
        public static MainViewModel Instance
        {
            get
            {
                return instance ?? (instance = new MainViewModel());
            }
        }
             
        public async void AutoLogin(Action callbackSuccess, Action callbackFail)
        {
            BaseViewModel viewModel = null;
            
            var userID = SettingsModule.Receive<string>(Constants.USER_ID);

            if (!string.IsNullOrEmpty(userID))
            {
                var userAccessToken = SettingsModule.Receive<string>(Constants.USER_ACCESS_TOKEN);

                if (!string.IsNullOrEmpty(userAccessToken))
                {
                    var userRole = SettingsModule.Receive<int>(Constants.USER_ROLE);

                    if (userRole > -1)
                    {
                        UserInfo = new UserCredentials() { Username = userID, AccessToken = userAccessToken, Role = (UserRoles)userRole }; 

                        switch (BaseViewModel.UserInfo.Role)
                        {
                            case UserRoles.User:

                                viewModel = UserMainViewModel.Instance; 

                                break;

                            case UserRoles.Admin:

                                viewModel = AdminMainViewModel.Instance; 

                                break;

                            case UserRoles.Moderator:

                                viewModel = ModeratorMainViewModel.Instance;

                                break;
                        }
                    }
                }
            }

            if (viewModel != null)
            {
                var result = await Task.Run<Result>(() => viewModel.Initialize());

                ProgressModule.End();

                if (result.HasError)
                {
                    AlertModule.ShowError(result.Message, () => AutoLogin(callbackSuccess, callbackFail));
                }
                else
                {
                    if (callbackSuccess != null)
                    {                   
                        DispatcherModule.Invoke(callbackSuccess);
                    }
                }
            }
            else
            {
                if (callbackFail != null)
                {                   
                    DispatcherModule.Invoke(callbackFail);
                }
            }
        }
            
        private static MainViewModel instance;
    }
}

