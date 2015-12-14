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
            
        public async void PushReportSpam(float latitude, float longitude, byte[] photo, Action callback, Action continueCallback)
        {
            var result = await Task.Run<Result<string>>(() => SourceManager.PushReportSpam(latitude, longitude, photo));

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => PushReportSpam(latitude, longitude, photo, callback, continueCallback));
            }
            else
            {
                if (result.IsSuccess)
                {
                    AlertModule.ShowInformation("Report sent and successfully processed. Thanks", callback);
                }
                else
                {
                    AlertModule.ShowWarning("Report sent successfully but can't analize photo.", continueCallback);
                }
            }
        }
               
        private static UserMainViewModel instance;
    }
}

