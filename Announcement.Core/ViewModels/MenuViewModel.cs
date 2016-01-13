﻿using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class MenuViewModel : BaseViewModel
    {
        public static MenuViewModel Instance
        {
            get
            {
                return instance ?? (instance = new MenuViewModel());
            }
        }
             
        public async void Logout( Action callback)
        {
            UserInfo = null;

            SettingsModule.Remove(Constants.USER_ID);

            SettingsModule.Remove(Constants.USER_ACCESS_TOKEN);

            SettingsModule.Remove(Constants.USER_ROLE);
            
            ProgressModule.Message(LocalizationModule.Translate("progress_logout"));

            await Task.Delay(1000);

            ProgressModule.End();

            if (callback != null)
            {
                DispatcherModule.Invoke(callback);
            }
        }

        private static MenuViewModel instance;
    }
}

