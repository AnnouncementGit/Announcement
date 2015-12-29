using Android.OS;
using Android.Views;
using Announcement.Android.Controls;
using Android.Content;
using Android.Widget;
using Android.Net;
using Java.Lang;
using Java.IO;
using Announcement.Core;
using Android.Media;

namespace Announcement.Android
{
	public class AddAudioFileFragment : BaseFragment
	{
		MediaPlayer mp;
		private Announcement.Android.Controls.TextView audioFileTextView;
		byte[] fileByte;
		string filePath;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.add_audiofile_view, null);

			audioFileTextView = view.FindViewById<Announcement.Android.Controls.TextView> (Resource.Id.audioFileTextView);
			audioFileTextView.Click += AudioFileTextViewOnClick;

			var btnUpload = view.FindViewById<Announcement.Android.Controls.Button> (Resource.Id.btnUpload);
			btnUpload.Click += BtnUploadOnClick;

			var btnPlay = view.FindViewById<ImageView> (Resource.Id.btnPlay);
			btnPlay.Click += BtnPlayOnClick;

			filePath = "http://www.ex.ua/load/9768738";

			mp = new MediaPlayer ();

			return view;
		}

		void BtnPlayOnClick (object sender, System.EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace (filePath))
				PlayAudioByPath (filePath);
			else
				AlertModule.ShowInformation ("No audio on server. Please upload or choose file to play!");
		}

		private void PlayAudioByPath(string path)
		{
			try
			{
				mp.SetDataSource(path);
				mp.Prepare();
			}
			catch (Exception ex)
			{
				ex.PrintStackTrace ();
			}

			if (mp.IsPlaying)
				mp.Reset();
			else
				mp.Start();
		}

		void AudioFileTextViewOnClick (object sender, System.EventArgs e)
		{
			ShowFileChooser ();
		}

		void BtnUploadOnClick (object sender, System.EventArgs e)
		{
			if (fileByte != null && fileByte.Length != 0)
				AlertModule.ShowInformation ("File Uploaded");
			else
				AlertModule.ShowInformation ("Please choose file to upload!");
		}

		public override void OnPause ()
		{
			base.OnPause ();

			mp.Stop ();
		}

		private static int FileSelectCode = 123;
		private void ShowFileChooser() {
			Intent intent = new Intent(Intent.ActionGetContent); 
			intent.SetType("audio/*"); 
			intent.AddCategory(Intent.CategoryOpenable);

			try {
				StartActivityForResult(Intent.CreateChooser(intent, "Select a File to Upload"), FileSelectCode);
			} catch (global::Android.Content.ActivityNotFoundException ex) {
				// Potentially direct the user to the Market with a Dialog
				Toast.MakeText(MainActivityInstance.Current, "Please install a File Manager.", 
					ToastLength.Short).Show();
			}
		}

		public override void OnActivityResult (int requestCode, int resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			if (requestCode == FileSelectCode) 
			{
				if (data == null || data.Data == null) {
					System.Console.WriteLine("File Error");
					return;
				}

				Uri uri = data.Data;
				System.Console.WriteLine("File Uri: " + uri.ToString());
				var path = GetPath (MainActivityInstance.Current, uri);
				System.Console.WriteLine("File Path: " + path);

				var file = new File (path);
				fileByte = new byte[(int)file.Length()];
				audioFileTextView.Text = file.Name;

				try
				{
					ProgressModule.Message("Please Wait");
					var fileInputStream = new FileInputStream(file);
					fileInputStream.ReadAsync(fileByte);
					fileInputStream.Close();
					filePath = path;
				}
				catch (Exception) 
				{
					
				}
				ProgressModule.End ();
			}
		}

		private static string GetPath(Context context, Uri uri) {
			var content = new Java.Lang.String ("content");
			var file = new Java.Lang.String ("file");
			if (content.EqualsIgnoreCase(uri.Scheme)) {
				string[] projection = { "_data" };

				try {
					var cursor = MainActivityInstance.Current.ContentResolver.Query(uri, projection, null, null, null);
					int column_index = cursor.GetColumnIndexOrThrow("_data");
					if (cursor.MoveToFirst()) {
						return cursor.GetString(column_index);
					}
				} catch (Exception) {
					// ignore
				}
			}
			else if (file.EqualsIgnoreCase(uri.Scheme)) {
				return uri.Path;
			}

			return null;
		}

		private void ConvertFile(string path)
		{
			
		}
	}
}

