using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Announcement.Core;
using Android.Widget;
using Android.Content;
using Android.Provider;
using System;

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

		private ToggleButton btnMenu;
		private ImageView btnReportSpam;
		private ImageView spamImageView;
		private const int cameraRequestCode = 987;
		private Java.IO.File file;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.user_main_layout, null);

			btnReportSpam = view.FindViewById<ImageView> (Resource.Id.btnReportSpam);
			btnReportSpam.Click += BtnReportSpamOnClick;

			btnMenu = view.FindViewById<ToggleButton> (Resource.Id.btnMenu);
			btnMenu.Click += BtnMenuOnClick;

			spamImageView = view.FindViewById<ImageView> (Resource.Id.spamImageView);

            return view;
        }

		void BtnReportSpamOnClick (object sender, System.EventArgs e)
		{
			//clearing ImageView
			spamImageView.SetImageBitmap (null);

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

					try {
						byte[] bitmapData = await ImageDecoderHelper.GetByteDataFromFile(file.AbsolutePath);

						if (MainActivityInstance.Current.lastKnownLocation == null)
							viewModel.PushReportSpam (0, 0, bitmapData, DataUploadedCallback, DataUploadedContinueCallback);
						else
							viewModel.PushReportSpam ((float)MainActivityInstance.Current.lastKnownLocation.Latitude, (float)MainActivityInstance.Current.lastKnownLocation.Longitude, bitmapData, DataUploadedCallback, DataUploadedContinueCallback);

						var scaledBitmap = await ImageDecoderHelper.ScaleBitmap (file.AbsolutePath, 1080, 1920);
						spamImageView.SetImageBitmap (scaledBitmap);

					} catch (Exception) {

					}
				}
			}
		}

		private void DataUploadedCallback()
		{
		}

		private void DataUploadedContinueCallback()
		{
		}

		void BtnMenuOnClick (object sender, System.EventArgs e)
		{
			//AlertModule.ShowInformation("Report sent and successfully processed. Thanks", null);
			//AlertModule.ShowWarning("Report sent successfully but can't analize photo.", null);
			//ProgressModule.Message ("Message");

			btnMenu.Checked = true;
			var menu = new PopupMenu(MainActivityInstance.Current, btnMenu);
			menu.MenuItemClick += OnMenuItemClick;
			menu.Menu.Add(0, 1, 0, "Logout");
			menu.DismissEvent += OnMenuDismissEvent;
			menu.Show ();
		}

		void OnMenuItemClick (object sender, PopupMenu.MenuItemClickEventArgs e)
		{
			switch (e.Item.ItemId) {

			case 1:
				
				break;
			}
		}

		void OnMenuDismissEvent (object sender, PopupMenu.DismissEventArgs e)
		{
			btnMenu.Checked = false;
		}
    }
}


