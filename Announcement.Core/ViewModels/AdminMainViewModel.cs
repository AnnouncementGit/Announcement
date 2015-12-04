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
            
        public async void InitializeModerators(Action callback)
        {
            var viewModel = ModeratorsViewModel.Instance;

            var result = await Task.Run<Result>(() => viewModel.Initialize());

            await Task.Delay(3000);

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.Show(result, () => InitializeModerators(callback));
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
        }

        public async void InitializeValidations(Action callback)
        {
            var viewModel = ModeratorsViewModel.Instance;

            var result = await Task.Run<Result>(() => viewModel.Initialize());

            await Task.Delay(3000);

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.Show(result, () => InitializeValidations(callback));
            }
            else
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            }
        }

        private static AdminMainViewModel instance;
    }
}

