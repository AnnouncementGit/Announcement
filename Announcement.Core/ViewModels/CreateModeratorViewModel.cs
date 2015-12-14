using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class CreateModeratorViewModel : BaseViewModel
    {
        public static CreateModeratorViewModel Instance
        {
            get
            {
                return instance ?? (instance = new CreateModeratorViewModel());
            }
        }

        public async void PushModerator(string username, string password, Action callback)
        {
            var result = await Task.Run<Result<string>>(() => SourceManager.PushModerator(username, password));

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => PushModerator(username, password, callback));
            }
            else
            {
                if (result.IsSuccess)
                {
                    AlertModule.ShowInformation("Moderator successfully created. Thanks", callback);
                }
                else
                {
                    AlertModule.ShowWarning("Can't create moderator. Retry", () => PushModerator(username, password, callback));
                }
            }
        }

        private static CreateModeratorViewModel instance;
    }
}

