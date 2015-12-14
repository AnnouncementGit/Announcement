﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Announcement.Android;

namespace Announcement.Core
{
    public class SourceManager
    {
        public static SourceManager Instance
        {
            get
            {
                return instance ?? (instance = new SourceManager());
            }
        }
            

        public Result<List<Moderator>> PullModerators()
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_receiving_moderators"));

            return AmazonModule.InvokeLambda<List<Moderator>>("PullModerators", null);
        }

        public Result<Ratings> PullRatings()
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_receiving_ratings"));

            return AmazonModule.InvokeLambda<Ratings>("PullRatings", null);
        }

        public Result<List<Report>> PullReports()
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_receiving_reports"));

            return AmazonModule.InvokeLambda<List<Report>>("PullReports", null);
        }

        public Result<string> PushModerator(string username, string password)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_creating_moderator"));

            var moderator = new ModeratorRegistration() { Username = username, Password = password };

            return AmazonModule.InvokeLambda<string>("PushModerator", moderator);
        }

        public Result<string> RejectReport(string id)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_report_rejecting"));

            return AmazonModule.InvokeLambda<string>("RejectReport", id);
        }

        public Result<string> ConfirmReport(string id, string phoneNumber)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_report_rejecting"));

            var report = new Report() { Id = id, PhoneNumber = phoneNumber };

            return AmazonModule.InvokeLambda<string>("ConfirmReport", report);
        }
            
        public Result<string> PushReportSpam(float latitude, float longitude, byte[] photo)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_send_report_spam"));

            var report = new SingleReport() { Latitude = latitude, Longitude = longitude, Photo = photo  };

            return AmazonModule.InvokeLambda<string>("PushReport", report);
        }

        public Result<string> PushReportContinue(string id, float latitude, float longitude, byte[] photo)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_send_report_spam"));

            var report = new SingleContinueReport() { id = id, Latitude = latitude, Longitude = longitude, Photo = photo  };

            return AmazonModule.InvokeLambda<string>("PushReportContinue", report);
        }


        private static SourceManager instance;
    }
}

