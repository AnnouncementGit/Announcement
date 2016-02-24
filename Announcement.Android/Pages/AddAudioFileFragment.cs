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
using System.Threading.Tasks;
using Android.Gms.Analytics;

namespace Announcement.Android
{
	public class AddAudioFileFragment : BaseFragment
	{
        private SpammerAudioRecordInfoViewModel ViewModel 
        { 
            get 
            { 
                return SpammerAudioRecordInfoViewModel.Instance; 
            }
        }

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate (Resource.Layout.add_audiofile_view, null);

			audioFileTextView = view.FindViewById<Announcement.Android.Controls.TextView> (Resource.Id.audioFileTextView);
			audioFileTextView.Click += AudioFileTextViewOnClick;

			var btnUpload = view.FindViewById<Announcement.Android.Controls.Button> (Resource.Id.btnUpload);
			btnUpload.Click += BtnUploadOnClick;

            btnPlay = view.FindViewById<global::Android.Widget.Button> (Resource.Id.btnPlay);
			btnPlay.Click += BtnPlayOnClick;

            filePath = ViewModel.CurrentAudioRecordPath;

			audioPlayer = new MediaPlayer ();

			MainActivity.GATracker.SetScreenName ("Add Audio File Fragment");
			MainActivity.GATracker.Send (new HitBuilders.ScreenViewBuilder ().Build ());

			if(NavigationManager.CurrentHeader != null && NavigationManager.CurrentHeader is HeaderFragment)
				(NavigationManager.CurrentHeader as HeaderFragment).ShowBack (true);
			
			return view;
		}

		protected void BtnPlayOnClick (object sender, System.EventArgs e)
		{
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                PlayAudioByPath(filePath);
            }
            else
            {
                AlertModule.ShowInformation(LocalizationModule.Translate("alert_message_no_audio"));
            }
		}

        protected void PlayAudioByPath(string path)
        {
            btnPlay.Selected = !btnPlay.Selected;

            Task.Run(() =>
                {
                    try
                    {
                        audioPlayer.SetDataSource(path);

                        audioPlayer.Prepare();
                    }
                    catch (Exception ex)
                    {
                        ex.PrintStackTrace();
                    }

                    if (audioPlayer.IsPlaying)
                    {
                        audioPlayer.Reset();
                    }
                    else
                    {
                        audioPlayer.Start();
                    }
                });       
        }

        protected void AudioFileTextViewOnClick (object sender, System.EventArgs e)
		{
			ShowFileChooser ();
		}

        protected void BtnUploadOnClick (object sender, System.EventArgs e)
		{
            if (!string.IsNullOrEmpty(filePath) && !filePath.Equals(ViewModel.CurrentAudioRecordPath))
            {
                ViewModel.PushAudioRecord(filePath, null);
            }
            else
            {
                AlertModule.ShowInformation(LocalizationModule.Translate("alert_message_choose_file_upload"));
            }
		}
            
		public override void OnPause ()
		{
			base.OnPause ();

			audioPlayer.Stop ();
		}

	
		private void ShowFileChooser()
        {
            Intent intent = new Intent(Intent.ActionGetContent); 

            intent.SetType("audio/*"); 

            intent.AddCategory(Intent.CategoryOpenable);

            try
            {
                StartActivityForResult(Intent.CreateChooser(intent, "Select a File to Upload"), FileSelectCode);
            }
            catch (global::Android.Content.ActivityNotFoundException ex)
            {
                AlertModule.ShowInformation("Please install a File Manager.");
            }
        }

		public override void OnActivityResult (int requestCode, int resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			if (requestCode == FileSelectCode) 
			{
				if (data == null || data.Data == null) 
                {
					AlertModule.ShowInformation ("File error!");

					return;
				}

				Uri uri = data.Data;

                filePath = GetPath (MainActivityInstance.Current, uri);

                using (var file = new File(filePath))
                {
                    if (file.Length() > AUDIO_FILE_MAX_SIZE)
                    {
                        filePath = null;

                        AlertModule.ShowInformation (LocalizationModule.Translate("alert_message_audio_file_too_large"));
                    }
                    
                    audioFileTextView.Text = file.Name;
                }

				ProgressModule.End();
			}
		}

		private static string GetPath(Context context, Uri uri)
        {
            var content = new Java.Lang.String("content");

            var file = new Java.Lang.String("file");

            if (content.EqualsIgnoreCase(uri.Scheme))
            {
                string[] projection = { "_data" };

                try
                {
                    var cursor = MainActivityInstance.Current.ContentResolver.Query(uri, projection, null, null, null);

                    int column_index = cursor.GetColumnIndexOrThrow("_data");

                    if (cursor.MoveToFirst())
                    {
                        return cursor.GetString(column_index);
                    }
                }
                catch (Exception)
                {
  
                }
            }
            else if (file.EqualsIgnoreCase(uri.Scheme))
            {
                return uri.Path;
            }

            return null;
        }

		private void ConvertFile(string path)
		{
			
		}

        private MediaPlayer audioPlayer;

        private Announcement.Android.Controls.TextView audioFileTextView;

        private string filePath;

        private global::Android.Widget.Button btnPlay;

        private static int FileSelectCode = 123;

        private const int AUDIO_FILE_MAX_SIZE = 20971520;
	}
}

