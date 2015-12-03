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
            

        public ActionResult<List<Moderator>> PullModerators()
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_receiving_data"));

            var result = new ActionResult<List<Moderator>>();

            List<Moderator> moderators = null;

            ///
            moderators = new List<Moderator>();

            for (int index = 0; index < 20; index++)
            {
                moderators.Add(new Moderator(){ Email = string.Format("moderator{0}@mail.com", index) });
            }

            Thread.Sleep(3000);
            ///
            result.Value = moderators;

            if (result.Value == null)
            {
                moderators = new List<Moderator>();
            }

            return result;
        }

        public ActionResult PushReportSpam(int latitude, int longitude, byte[] buffer)
        {
            ProgressModule.Message(LocalizationModule.Translate("progress_send_report_spam"));

            var result = new ActionResult();

            List<Moderator> moderators = null;

            Thread.Sleep(3000);

            return result;
        }


        private static SourceManager instance;
    }
}

