using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class ModeratorMainViewModel : BaseViewModel
    {
        public List<User> RatingTopUsers { get; private set; }

        public List<Spammer> RatingTopSpammers { get; private set; }

        public List<Report> Reports { get; private set; }
        
        public static ModeratorMainViewModel Instance
        {
            get
            {
                return instance ?? (instance = new ModeratorMainViewModel());
            }
        }


        public override Result Initialize()
        {
            var result = base.Initialize();

            var resultReports = SourceManager.PullReports();

            if (resultReports.HasError)
            {
                return resultReports;
            }
            else
            {
                Reports = resultReports.Value;
            }

            var resultRatings = SourceManager.PullRatings();

            if (resultRatings.HasError)
            {
                return resultRatings;
            }
            else
            {
                var ratings = resultRatings.Value;

                if (ratings != null)
                {
                    RatingTopUsers = ratings.TopUsers;

                    RatingTopSpammers = ratings.TopSpammers;
                }
            }

            return result;
        }

        public async void InitializeReportValidation(Report report, Action callback)
        {
            var viewModel = ReportValidationViewModel.Instance;

            var result = await Task.Run<Result>(() => viewModel.Initialize(report));

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => InitializeReportValidation(report, callback));
            }
            else
            {
                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }
            
        private static ModeratorMainViewModel instance;
    }
}

