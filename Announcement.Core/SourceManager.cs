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

            var result = AmazonModule.InvokeLambda<List<Moderator>>("PullModerators", null);

            if (result.Value == null)
            {
                result.Value = new List<Moderator>();
            }

            return result;
        }

        public Result<Ratings> PullRatings()
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_receiving_ratings"));

            var result = AmazonModule.InvokeLambda<Ratings>("PullRatings", null);

            var ratings = result.Value;

            if (ratings.TopUsers == null)
            {
                ratings.TopUsers = new List<User>();
            }

            if (ratings.TopSpammers == null)
            {
                ratings.TopSpammers = new List<Spammer>();
            }

            return result;
        }

        public Result<List<Report>> PullReports()
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_receiving_reports"));

            var result = AmazonModule.InvokeLambda<List<Report>>("PullReports", null);

            if (result.Value == null)
            {
                result.Value = new List<Report>();
            }

            return result;
        }

        public Result<string> PushModerator(string username, string password)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_creating_moderator"));

            var moderator = new ModeratorRegistration() { Username = username, Password = password };

            return AmazonModule.InvokeLambda<string>("PushModerator", moderator);
        }

        public Result<string> DeleteModerator(string id)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_deleting_moderator"));
            
            return AmazonModule.InvokeLambda<string>("RemoveModerator", id);
        }

        public Result<string> RejectReport(string id)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_report_rejecting"));

            return AmazonModule.InvokeLambda<string>("RejectReport", id);
        }

        public Result<string> ConfirmReport(string id, string phoneNumber)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_report_confirming"));

            var report = new Report() { Id = id, PhoneNumber = phoneNumber };

            return AmazonModule.InvokeLambda<string>("ConfirmReport", report);
        }
            
        public Result<string> PushReportSpam(float latitude, float longitude, byte[] photo)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_send_report_spam"), true);

            var report = new SingleReport() { Latitude = latitude, Longitude = longitude, Photo = photo  };

            return AmazonModule.InvokeLambda<string>("PushReport", report);
        }

        public Result<string> PushReportContinue(string id, float latitude, float longitude, byte[] photo)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_send_report_spam"), true);

            var report = new SingleContinueReport() { id = id, Latitude = latitude, Longitude = longitude, Photo = photo  };

            return AmazonModule.InvokeLambda<string>("PushReportContinue", report);
        }


        private static SourceManager instance;
    }
}

