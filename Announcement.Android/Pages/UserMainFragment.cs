using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Announcement.Core;
using Android.Widget;
using Android.Content;
using Android.Provider;
using System;
using System.Threading.Tasks;
using System.Threading;
using Java.IO;

namespace Announcement.Android
{
	public class UserMainFragment : Fragment
	{
		private UserMainViewModel viewModel 
		{ 
            get
            { 
                return UserMainViewModel.Instance; 
            }
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.user_main_layout, null);

            btnReportSpam = view.FindViewById<ImageView>(Resource.Id.btnReportSpam);

            btnReportSpam.Click += BtnReportSpamOnClick;

            return view;
        }

        protected void BtnReportSpamOnClick(object sender, EventArgs e)
        {
            if (!LocationService.IsLocationEnabled)
            {              
                AlertModule.ShowInformation(LocalizationModule.Translate("alert_enable_gps"), OpenSettigs);

                return;
            }
                
            var place = new File(global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryPictures), "Announcement");

            if (!place.Exists())
            {
                place.Mkdirs();
            }

            using (var file = new File(place, String.Format(PHOTO_NAME_TEMPLATE, Guid.NewGuid())))
            {
                photoPath = file.AbsolutePath;

                var intent = new Intent(MediaStore.ActionImageCapture);

                intent.PutExtra(MediaStore.ExtraOutput, global::Android.Net.Uri.FromFile(file));

                StartActivityForResult(intent, CAMERA_REQUEST_CODE);
            }
        }

		public override async void OnActivityResult (int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
           
            if (requestCode == CAMERA_REQUEST_CODE && resultCode == (int)global::Android.App.Result.Ok)
            {
                ProgressModule.Message(LocalizationModule.Translate("progress_getting_photo"), true);
                
                btnReportSpam.Visibility = ViewStates.Gone;

                var buffer = await Task.Run<byte[]>(() => ImageDecoderHelper.FilePathToByteArray(photoPath, PHOTO_MAX_SIZE));

                if (buffer != null)
                {
                    ReportSpam(buffer); 
                }
            }
        }

        protected async void ReportSpam(byte[] buffer)
        {
            if (!LocationService.IsLocationEnabled)
            {              
				AlertModule.ShowInformation(LocalizationModule.Translate("alert_enable_gps"), OpenSettigs);

                return;
            }

            ProgressModule.Message(LocalizationModule.Translate("progress_trying_to_get_location"), true);
            
            var locationTask = LocationService.GetLocationAsync();

            Task.Run<bool>(() =>
                {
                   return locationTask.Wait(5000);
                }).ContinueWith((result) =>
                {
                    if (result.Result)
                    {
                        var location = locationTask.Result;
                    
                        viewModel.PushReportSpam((float)location.Latitude, (float)location.Longitude, buffer, PushReportSpamCallback, PushReportSpamContinueCallback, PushReportSpamCancelCallback);
                    }
                    else
                    {
                        ProgressModule.End();
                    
                        AlertModule.ShowWarning(LocalizationModule.Translate("alert_bad_location"), async () => ReportSpam(buffer), PushReportSpamCancelCallback);
                    }
                });
        }

        private void PushReportSpamCancelCallback()
		{
			btnReportSpam.Visibility = ViewStates.Visible;
		}

        private void PushReportSpamCallback()
		{
			btnReportSpam.Visibility = ViewStates.Visible;
		}

        private void PushReportSpamContinueCallback()
		{
			//btnReportSpam.Visibility = ViewStates.Visible;

            btnReportSpam.PerformClick();
		}

		private void OpenSettigs()
		{
			Intent intent = new Intent (Settings.ActionLocationSourceSettings);
			Activity.StartActivity (intent);
		}


        private ImageView btnReportSpam;

        private const int PHOTO_MAX_SIZE = 1080;

        private const int CAMERA_REQUEST_CODE = 987;

        private const int LOCATION_TIMEOUT_MS = 4000;

        private const string PHOTO_NAME_TEMPLATE = "{0}.png";

        private string photoPath;
	}
}


