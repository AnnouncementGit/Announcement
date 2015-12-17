using System;
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
             
        protected async void Logout( Action callback)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_logout"));

            await Task.Delay(1000);

            ProgressModule.End();

            if (callback != null)
            {
                callback.Invoke();
            }
        }

        private static MenuViewModel instance;
    }
}

