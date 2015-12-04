using System;
using System.Threading.Tasks;
using Announcement.Android;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class UserMainViewModel : BaseViewModel
    {
        public static UserMainViewModel Instance
        {
            get
            {
                return instance ?? (instance = new UserMainViewModel());
            }
        }
            
        public async void PushReportSpam(int latitude, int longitude, byte[] buffer, Action callback)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            var result = await Task.Run<Result>(() => SourceManager.PushReportSpam(latitude, longitude, buffer));

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.Show(result, () => PushReportSpam(latitude, longitude, buffer, callback));
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
        }
               
        private static UserMainViewModel instance;
    }
}

