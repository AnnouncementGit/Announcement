using System;
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

        public Result<object> PushModerator(string username, string password)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_creating_moderator"));

            var moderator = new ModeratorRegistration() { Username = username, Password = password };

            return AmazonModule.InvokeLambda<object>("PushModerator", moderator);
        }

        public Result<object> RejectReport(int id)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_report_rejecting"));

            return AmazonModule.InvokeLambda<object>("RejectReport", id);
        }

        public Result<object> ConfirmReport(int id, string phoneNumber, string audioRecord)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_report_rejecting"));

            var report = new Report() { Id = id, PhoneNumber = phoneNumber, AudioRecord = audioRecord };

            return AmazonModule.InvokeLambda<object>("ConfirmReport", report);
        }
            
        public Result<int> PushReportSpam(float latitude, float longitude, byte[] photo)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_send_report_spam"));

            var report = new SingleReport() { Latitude = latitude, Longitude = longitude, Photo = photo  };

            return AmazonModule.InvokeLambda<int>("PushReport", report);
        }


        private static SourceManager instance;
    }
}

