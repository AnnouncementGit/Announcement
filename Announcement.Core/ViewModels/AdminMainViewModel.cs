﻿using System;
using System.Linq;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class AdminMainViewModel : BaseViewModel
    {
        public List<Moderator> Moderators { get; private set; }

        public List<User> RatingTopUsers { get; private set; }

        public List<SpammerShort> RatingTopSpammers { get; private set; }

        public List<Report> Reports { get; private set; }
        
        public static AdminMainViewModel Instance
        {
            get
            {
                return instance ?? (instance = new AdminMainViewModel());
            }
        }


        public override Result Initialize()
        {
            var result = base.Initialize();

            var resultModerators = SourceManager.PullModerators();

            if (resultModerators.HasError)
            {
                return resultModerators;
            }
            else
            {
                Moderators = resultModerators.Value;
            }

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
                else
                {
                    RatingTopUsers = new List<User>();

                    RatingTopSpammers = new List<SpammerShort>();
                }
            }

            return result;
        }

        public async void DeleteModerator(Moderator moderator, Action<Moderator, bool> callback)
        {
            AlertModule.Show(LocalizationModule.Translate("alert_title_type_information"), LocalizationModule.Translate("alert_message_delete_question"), LocalizationModule.Translate("alert_button_delete"), LocalizationModule.Translate("button_title_cancel"), async () =>
                {
                    var result = await Task.Run<Result<string>>(() => SourceManager.DeleteModerator(moderator.Username));

                    ProgressModule.End();

                    if (result.HasError || !result.IsSuccess)
                    {
                        AlertModule.ShowError(result.Message, () => DeleteModerator(moderator, callback), () => DispatcherModule.Invoke<Moderator, bool>(callback, moderator, false));
                    }
                    else
                    {
                        AlertModule.Show(LocalizationModule.Translate("alert_title_type_information"), LocalizationModule.Translate("alert_message_moderator_delete_completed"), LocalizationModule.Translate("alert_button_ok"),
                            () =>
                            { 
                                DispatcherModule.Invoke<Moderator, bool>(callback, moderator, true);
                            });
                    }
                },
                () =>
                {
                    DispatcherModule.Invoke<Moderator, bool>(callback, moderator, false);
                });
        }

        public async void InitializeCreateModerator(Action callback)
        {
            var viewModel = CreateModeratorViewModel.Instance;

            var result = await Task.Run<Result>(() => viewModel.Initialize());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => InitializeCreateModerator(callback));
            }
            else
            {
                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
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

        public async void InitializeSpammers(Action callback)
        {
            var viewModel = SpammersMainViewModel.Instance;

            var result = await Task.Run<Result>(() => viewModel.Initialize());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => InitializeSpammers(callback));
            }
            else
            {
                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }

        public async void RefreshModerators(Action callback)
        {
            var result = await Task.Run<Result<List<Moderator>>>(() => SourceManager.PullModerators());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => RefreshModerators(callback));
            }
            else
            {
                Moderators = result.Value;
                
                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }

        public async void RefreshReports(Action callback)
        {
            var result = await Task.Run<Result<List<Report>>>(() => SourceManager.PullReports());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => RefreshReports(callback));
            }
            else
            {
                Reports = result.Value;

                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }

        public async void RefreshRatings(Action callback)
        {
            var result = await Task.Run<Result<Ratings>>(() => SourceManager.PullRatings());

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => RefreshRatings(callback));
            }
            else
            {
                var ratings = result.Value;

                if (ratings != null)
                {
                    RatingTopUsers = ratings.TopUsers;

                    RatingTopSpammers = ratings.TopSpammers;
                }

                if (callback != null)
                {
                    DispatcherModule.Invoke(callback);
                }
            }
        }


        private static AdminMainViewModel instance;
    }
}

