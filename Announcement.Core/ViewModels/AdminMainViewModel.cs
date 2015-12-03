using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class AdminMainViewModel : BaseViewModel
    {
        public static AdminMainViewModel Instance
        {
            get
            {
                return instance ?? (instance = new AdminMainViewModel());
            }
        }
            
        public AdminMainViewModel()
        {
            
        }

        public override InitializationStatus Initialize()
        {
            var status = new InitializationStatus() { IsSuccess = true };

            Erase();

            var pullModeratorsResult = SourceManager.PullModerators();

            if (pullModeratorsResult.HasError)
            {
                status.IsSuccess = false;

                status.Message = pullModeratorsResult.Message;

                return status;
            }

            return status;
        }

        public override void Erase()
        {
            
        }
            
        public async void OpenModerators(Action callback)
        {
            var viewModel = ModeratorsViewModel.Instance;

            var status = await Task.Run<InitializationStatus>(() => viewModel.Initialize());

            await Task.Delay(5000);

            ProgressModule.End();

            if (status.IsSuccess)
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
            else
            {
                //AlertModule.Show(LocalizationModule.Translate("title_key"), status.Message, LocalizationModule.Translate("try_again_key"), () => NavigateToUsersView(callback), null);
            }
        }

        private static AdminMainViewModel instance;
    }
}

