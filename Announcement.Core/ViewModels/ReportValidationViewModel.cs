using System;
using Announcement.Android;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class ReportValidationViewModel : BaseViewModel
    {
        public String PhoneNumber { get; set; }

        public List<string> Photos { get; set; }

		public float Latitude { get; set;}

		public float Longitude { get; set;}


        public static ReportValidationViewModel Instance
        {
            get
            {
                return instance ?? (instance = new ReportValidationViewModel());
            }
        }


        public Result Initialize(Report report)
        {
            var result = base.Initialize();

            currentReport = report;

            if (report != null)
            {
                PhoneNumber = report.PhoneNumber != null ? report.PhoneNumber : string.Empty;

                Photos = report.Photos != null ? report.Photos : new List<string>();

				Latitude = report.Latitude;
				Longitude = report.Longitude;
            }
            else
            {
                result.HasError = true;

                result.Message = LocalizationModule.Translate("alert_message_report_null");
            }

            return result;
        }

        public async void ConfirmReport(string phoneNumber, Action callback)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                AlertModule.Show(LocalizationModule.Translate("alert_title_type_error"), LocalizationModule.Translate("alert_message_empty_phone_number"), LocalizationModule.Translate("alert_button_ok"));

                return;
            }
            
            PhoneNumber = phoneNumber;

            var result = await Task.Run<Result<string>>(() => SourceManager.ConfirmReport(currentReport.Id, phoneNumber));

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => ConfirmReport(phoneNumber, callback));
            }
            else
            {
                if (result.IsSuccess)
                {
                    List<Report> reports = null;
                    
                    if (UserInfo.Role == UserRoles.Admin)
                    {
                        reports = AdminMainViewModel.Instance.Reports;
                    }
                    else
                    {
                        reports = ModeratorMainViewModel.Instance.Reports;
                    }

                    reports.Remove(currentReport);

                    AlertModule.ShowInformation(LocalizationModule.Translate("alert_message_report_confirmed"), callback);
                }
                else
                {
                    AlertModule.ShowWarning(LocalizationModule.Translate("alert_message_report_not_confirmed"), () => ConfirmReport(phoneNumber, callback));
                }
            }
        }

        public async void RejectReport(Action callback)
        {
            var result = await Task.Run<Result<string>>(() => SourceManager.RejectReport(currentReport.Id));

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => RejectReport(callback));
            }
            else
            {
                if (result.IsSuccess)
                {
                    List<Report> reports = null;

                    if (UserInfo.Role == UserRoles.Admin)
                    {
                        reports = AdminMainViewModel.Instance.Reports;
                    }
                    else
                    {
                        reports = ModeratorMainViewModel.Instance.Reports;
                    }

                    reports.Remove(currentReport);
                    
                    AlertModule.ShowInformation(LocalizationModule.Translate("alert_message_report_rejected"), callback);
                }
                else
                {
                    AlertModule.ShowWarning(LocalizationModule.Translate("alert_message_report_not_rejected"), () => RejectReport(callback));
                }
            }
        }

        private Report currentReport;

        private static ReportValidationViewModel instance;
    }
}

