using System;
using System.Threading.Tasks;
using Announcement.Android;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class SpammerAudioRecordInfoViewModel : BaseViewModel
    {
        public static SpammerAudioRecordInfoViewModel Instance
        {
            get
            {
                return instance ?? (instance = new SpammerAudioRecordInfoViewModel());
            }
        }

        public string CurrentAudioRecordPath { get; set; }

        public Result Initialize(Spammer spammer)
        {
            var result = base.Initialize();

            currentSpammer = spammer;

            CurrentAudioRecordPath = "https://s3-eu-west-1.amazonaws.com/stop-spam/audio_records/" + spammer.AudioRecord;
                
            return result;
        }
            
        public async void PushAudioRecord(string filePath, Action callback)
        {
            Result<Object> result = await Task.Run<Result<Object>>(() => SourceManager.PushAudioRecord(filePath, currentSpammer));  

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => PushAudioRecord(filePath, callback));
            }
            else
            {
                if (result.IsSuccess)
                {
                    AlertModule.Show(LocalizationModule.Translate("alert_title_upload_completed"), LocalizationModule.Translate("alert_message_upload_audio_file_completed"), LocalizationModule.Translate("alert_button_ok"), callback);
                }
            }
        }

        private Spammer currentSpammer;

        private static SpammerAudioRecordInfoViewModel instance;
    }
}

