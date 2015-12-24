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
			if (!EnteredDataValid (username, password))
				return;
			
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

		private bool EnteredDataValid(string username, string password)
		{
			if (string.IsNullOrWhiteSpace (username)) 
			{
				AlertModule.ShowInformation (LocalizationModule.Translate("alert_please_enter_name"));
				return false;
			}

			if (string.IsNullOrWhiteSpace (username) || username.Length < 5) 
			{
				AlertModule.ShowInformation (LocalizationModule.Translate("alert_name_too_short"));
				return false;
			}

			if (string.IsNullOrWhiteSpace (username) || username.Length < 5 || !ValidationModule.ValidateUserName (username)) 
			{
				AlertModule.ShowInformation (LocalizationModule.Translate("alert_name_can_have_only_letters_or_numbers"));
				return false;
			}

			if (string.IsNullOrWhiteSpace (password)) 
			{
				AlertModule.ShowInformation (LocalizationModule.Translate("alert_please_enter_password"));
				return false;
			}

			if (string.IsNullOrWhiteSpace (password) || password.Length < 5) 
			{
				AlertModule.ShowInformation (LocalizationModule.Translate("alert_password_too_short"));
				return false;
			}

			return true;
		}

        private static CreateModeratorViewModel instance;
    }
}

