﻿using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Announcement.Core;
using Android.Widget;
using Android.Content;
using Android.Provider;
using System;
using System.Threading.Tasks;
using System.Threading;

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

		private ImageView btnReportSpam;
		private const int cameraRequestCode = 987;
		private Java.IO.File file;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.user_main_layout, null);

			btnReportSpam = view.FindViewById<ImageView> (Resource.Id.btnReportSpam);
			btnReportSpam.Click += BtnReportSpamOnClick;

			return view;
		}

		void BtnReportSpamOnClick (object sender, System.EventArgs e)
		{
			if (!MainActivityInstance.Current.locationProvider.networkProviderEnabled && !MainActivityInstance.Current.locationProvider.gpsProviderEnabled) 
			{
				AlertModule.ShowInformation ("Please enable GPS and Network Positioning in your Settings", null);
				return;
			}

			Intent intent = new Intent (MediaStore.ActionImageCapture);
			Java.IO.File place = new Java.IO.File(global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryPictures), "Announcement");
			if (!place.Exists())
				place.Mkdirs ();

			file = new Java.IO.File (place, String.Format ("myPhoto_{0}.png", Guid.NewGuid ()));
			intent.PutExtra (MediaStore.ExtraOutput, global::Android.Net.Uri.FromFile (file));
			StartActivityForResult (intent, cameraRequestCode);
		}

		public async override void OnActivityResult (int requestCode, int resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			if (resultCode != (int)global::Android.App.Result.Ok) {
				file = null;
			} else {
				if (requestCode == cameraRequestCode) {
					btnReportSpam.Visibility = ViewStates.Gone;
					try {
						var imageByteArray = await ImageDecoderHelper.GetByteDataFromFile(file.AbsolutePath);

						ReportSpam(imageByteArray); // try to find info by deecoding to exsacly image size

						await ImageDecoderHelper.ScaleBitmap (file.AbsolutePath, 1080, 1920);

					} catch (Exception) {

					}
				}
			}
		}

		private void ReportSpam(byte[] byteArray, int progressTime = 0)
		{
			if (progressTime > 0) {
				Task.Run (() => {
					ProgressModule.Message ("Trying to get  your current location...", true);
					Thread.Sleep (progressTime * 1000);
				}).ContinueWith ((task) => {
					MainActivityInstance.Current.RunOnUiThread(()=>{
						ProgressModule.End ();
						ReportSpam (byteArray);});
				});
			} else {
				if (MainActivityInstance.Current.lastKnownLocation == null)
					AlertModule.ShowWarning ("Bad current location.\nTry again?", () => ReportSpam (byteArray, 4), DataUploadedCancelCallback);
				else
					viewModel.PushReportSpam ((float)MainActivityInstance.Current.lastKnownLocation.Latitude, (float)MainActivityInstance.Current.lastKnownLocation.Longitude, byteArray, DataUploadedCallback, DataUploadedContinueCallback, DataUploadedCancelCallback);
			}
		}

		private void DataUploadedCancelCallback()
		{
			btnReportSpam.Visibility = ViewStates.Visible;
		}

		private void DataUploadedCallback()
		{
			btnReportSpam.Visibility = ViewStates.Visible;
		}

		private void DataUploadedContinueCallback()
		{
			btnReportSpam.Visibility = ViewStates.Visible;
		}
	}
}


