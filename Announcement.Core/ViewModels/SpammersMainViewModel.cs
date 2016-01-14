using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class SpammersMainViewModel : BaseViewModel
    {
        public List<Spammer> Spammers { get; private set; }

        public static SpammersMainViewModel Instance
        {
            get
            {
                return instance ?? (instance = new SpammersMainViewModel());
            }
        }


        public override Result Initialize()
        {
            var result = base.Initialize();


            var resultReports = SourceManager.PullSpammers();

            if (resultReports.HasError)
            {
                return resultReports;
            }
            else
            {
                Spammers = resultReports.Value;
            }
                
            return result;
        }
            
        public async void InitializeAudioRecordForSpammer(Spammer spammer, Action callback)
        {
            var viewModel = SpammerAudioRecordInfoViewModel.Instance;

            var result = await Task.Run<Result>(() => viewModel.Initialize(spammer));

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => InitializeAudioRecordForSpammer(spammer, callback));
            }
            else
            {
                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }

        public async void RefreshSpammers(Action callback)
        {
            var result = await Task.Run<Result<List<Spammer>>>(() => SourceManager.PullSpammers());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => RefreshSpammers(callback));
            }
            else
            {
                Spammers = result.Value;

                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }


        private static SpammersMainViewModel instance;
    }
}

