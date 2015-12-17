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
                    
                    AlertModule.Show(LocalizationModule.Translate("alert_title_upload_completed"), LocalizationModule.Translate("alert_message_upload_completed"),  LocalizationModule.Translate("alert_button_ok"), callback);
                }
                else
                {
                    reportId = result.Value;
                    
                    AlertModule.Show(LocalizationModule.Translate("alert_title_upload_completed"), LocalizationModule.Translate("alert_message_upload_completed_not_found"), LocalizationModule.Translate("alert_button_retry"), LocalizationModule.Translate("alert_button_cancel"), continueCallback, cancelCallback);
                }
            }
        }

        private string reportId;
               
        private static UserMainViewModel instance;
    }
}

