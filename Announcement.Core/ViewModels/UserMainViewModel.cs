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
            
        public UserMainViewModel()
        {
            
        }

        public override InitializationStatus Initialize()
        {
            return base.Initialize();
        }

        public async void PushReportSpam(int latitude, int longitude, byte[] buffer, Action callback)
        {
            //
            //TODO
            //
            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            var status = await Task.Run<ActionResult>(() => SourceManager.PushReportSpam(latitude, longitude, buffer));

            ProgressModule.End();

            if (status.HasError)
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
            else
            {
                AlertModule.Show(AlertModule.INFORMATION, status.Message, LocalizationModule.Translate("alert_try_again"), () => PushReportSpam(latitude, longitude, buffer, callback));
            }
        }
               
        private static UserMainViewModel instance;
    }
}

