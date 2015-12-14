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
            }
            else
            {
                result.HasError = true;

                result.Message = LocalizationModule.Translate("alert_message_report_null");
            }

            return result;
        }

        public async void ConfirmReport(Action callback)
        {
            var result = await Task.Run<Result<string>>(() => SourceManager.ConfirmReport(currentReport.Id, PhoneNumber));

            ProgressModule.End();

            if (result.HasError)
            {
                AlertModule.ShowError(result.Message, () => ConfirmReport(callback));
            }
            else
            {
                if (result.IsSuccess)
                {
                    AlertModule.ShowInformation("Report successfully confirmed. Thanks", callback);
                }
                else
                {
                    AlertModule.ShowWarning("Report not confirmed", () => ConfirmReport(callback));
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
                    AlertModule.ShowInformation("Report successfully rejected. Thanks", callback);
                }
                else
                {
                    AlertModule.ShowWarning("Report not rejected", () => RejectReport(callback));
                }
            }
        }

        private Report currentReport;

        private static ReportValidationViewModel instance;
    }
}

