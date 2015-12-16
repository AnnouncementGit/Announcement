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
            
        public async void PushReportSpam(float latitude, float longitude, byte[] photo, Action callback, Action continueCallback, Action cancelCallback)
        {
            Result<string> result = null;
            
            if (string.IsNullOrEmpty(reportId))
            {
                result = await Task.Run<Result<string>>(() => SourceManager.PushReportSpam(latitude, longitude, photo));  
            }
            else
            {
                result = await Task.Run<Result<string>>(() => SourceManager.PushReportContinue(reportId, latitude, longitude, photo));  
            }

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => PushReportSpam(latitude, longitude, photo, callback, continueCallback, cancelCallback), cancelCallback);
            }
            else
            {
                if (result.IsSuccess)
                {
                    reportId = null;
                    
                    AlertModule.ShowInformation("Report sent and successfully processed. Thanks", callback);
                }
                else
                {
                    reportId = result.Value;
                    
                    AlertModule.ShowWarning("Report sent successfully but can't analize photo.", continueCallback, cancelCallback);
                }
            }
        }

        private string reportId;
               
        private static UserMainViewModel instance;
    }
}

