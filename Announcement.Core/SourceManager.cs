using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Announcement.Android;
using System.Linq;

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

            var result = AmazonModule.InvokeLambda<List<Moderator>>("PullModerators", BaseViewModel.UserInfo);

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

            if (ratings == null)
            {
                ratings = new Ratings();
            }

            if (ratings.TopUsers == null)
            {
                ratings.TopUsers = new List<User>();
            }

            if (ratings.TopSpammers == null)
            {
                ratings.TopSpammers = new List<SpammerShort>();
            }
            else
            {
                ratings.TopSpammers = ratings.TopSpammers.OrderByDescending(s => s.SpamCount).ToList();
            }

            return result;
        }

        public Result<List<Report>> PullReports()
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_receiving_reports"));

            var result = AmazonModule.InvokeLambda<List<Report>>("PullReports", BaseViewModel.UserInfo);

            if (result.Value == null)
            {
                result.Value = new List<Report>();
            }

            return result;
        }

        public Result<string> PushModerator(string username, string password)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_creating_moderator"));

            var moderator = new ModeratorRegistration(BaseViewModel.UserInfo) { ModeratorUsername = username, ModeratorPassword = password };

            return AmazonModule.InvokeLambda<string>("PushModerator", moderator);
        }

        public Result<string> DeleteModerator(string id)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_deleting_moderator"));

            var optionId = new OptionId(BaseViewModel.UserInfo) { Id = id }; 

            return AmazonModule.InvokeLambda<string>("RemoveModerator", optionId);
        }

        public Result<string> RejectReport(string id)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_report_rejecting"));

            var optionId = new OptionId(BaseViewModel.UserInfo) { Id = id }; 

            return AmazonModule.InvokeLambda<string>("RejectReport", optionId);
        }

        public Result<string> ConfirmReport(string id, string phoneNumber)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_report_confirming"));

            var report = new OptionReport(BaseViewModel.UserInfo) { Id = id, PhoneNumber = phoneNumber };

            return AmazonModule.InvokeLambda<string>("ConfirmReport", report);
        }
            
        public Result<string> PushReportSpam(float latitude, float longitude, byte[] photo)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_send_report_spam"), true);

            var report = new OptionReport(BaseViewModel.UserInfo) { Latitude = latitude, Longitude = longitude, Photo = photo  };

            return AmazonModule.InvokeLambda<string>("PushReport", report);
        }

        public Result<string> PushReportContinue(string id, float latitude, float longitude, byte[] photo)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_send_report_spam"), true);

            var report = new OptionReport(BaseViewModel.UserInfo) { Id = id, Latitude = latitude, Longitude = longitude, Photo = photo  };

            return AmazonModule.InvokeLambda<string>("PushReportContinue", report);
        }

        public Result<UserCredentials> Login(string username, string password)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            var user = new User();

            user.Username = username;

            user.Password = password;

            return AmazonModule.InvokeLambda<UserCredentials>("Login", user);
        }

		public Result<UserCredentials> LoginViaSocial(string userId, string userName, string token)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_authentication"));

            var user = new SocialUser();

            user.UserId = userId;

			user.Username = userName;

            user.Token = token;

            return AmazonModule.InvokeLambda<UserCredentials>("LoginViaSocial", user);
        }

        public Result<List<Spammer>> PullSpammers()
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_receiving_spammers"));

            var result = AmazonModule.InvokeLambda<List<Spammer>>("PullSpammers", BaseViewModel.UserInfo);

            if (result.Value == null)
            {
                result.Value = new List<Spammer>();
            }

            return result;
        }
            
        public Result<Object> PushAudioRecord(string filePath, Spammer spammer)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_uploading_audio_record"));

            var fileName = string.Format("{0}.mp3", spammer.PhoneNumber);

            if (AmazonModule.UploadAudioFile(filePath, fileName).Result)
            {
                var result = AmazonModule.InvokeLambda<Object>("AssignAudioRecordWithSpammer", new OptionSpammer(BaseViewModel.UserInfo) { Id = spammer.PhoneNumber, AudioRecord = fileName });

                if (result.IsSuccess)
                {
                    spammer.AudioRecord = fileName;
                }

                return result;
            }
            else
            {
                return new Result<Object>() { HasError = true };
            }
        }
            
        private static SourceManager instance;
    }
}

