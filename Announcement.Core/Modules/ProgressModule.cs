using System;
using Android.App;
using Android.Views;
using Announcement.Android;
using System.Threading.Tasks;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using System.Collections.Generic;

namespace Announcement.Core
{
	public class ProgressModule
	{
		public static void Message(string message)
		{
			Application.SynchronizationContext.Post (ignored => {
				var activity = NavigationManager.CurrentActivity;

				if (activity == null)
					return;

				if (progressView == null) {
					var customView = activity.LayoutInflater.Inflate (Resource.Layout.simple_progress, null);
					ProgressDialog.Builder builder = new ProgressDialog.Builder (activity);
					builder.SetView (customView);

					progressView = builder.Create ();

					progressView.SetCancelable (false);

					progressView.SetCanceledOnTouchOutside (false);

					if (!progressView.IsShowing)
						progressView.Show ();

					//initAnimation
					Bitmap b = BitmapFactory.DecodeResource (MainActivityInstance.Current.Resources, Resource.Drawable.progress_loading);
					AnimationDrawable shiftedAnimation = GetAnimation (b);

					View v = progressView.FindViewById<View> (Resource.Id.progressView);
					v.Background = (shiftedAnimation);
					shiftedAnimation.Start ();
				}

				progressView.FindViewById<TextView> (Resource.Id.progressTitle).Text = message;

				if (!progressView.IsShowing)
					progressView.Show ();

			}, null);
		}

		public static void End()
        {
            Application.SynchronizationContext.Post(ignored =>
                {
                    var activity = NavigationManager.CurrentActivity;

                    if (activity == null)
                        return;
            
                    if (progressView != null)
                    {
                        progressView.Dismiss();
                    }

                }, null);
        }

		private static Bitmap GetShiftedBitmap(Bitmap bitmap, int shiftX) {
			Bitmap newBitmap = Bitmap.CreateBitmap (bitmap.Width, bitmap.Height, bitmap.GetConfig ());
			Canvas newBitmapCanvas = new Canvas (newBitmap);

			Rect srcRect1 = new Rect (shiftX, 0, bitmap.Width, bitmap.Height);
			Rect destRect1 = new Rect (srcRect1);
			destRect1.Offset (-shiftX, 0);
			newBitmapCanvas.DrawBitmap (bitmap, srcRect1, destRect1, null);

			Rect srcRect2 = new Rect (0, 0, shiftX, bitmap.Height);
			Rect destRect2 = new Rect (srcRect2);
			destRect2.Offset (bitmap.Width - shiftX, 0);
			newBitmapCanvas.DrawBitmap (bitmap, srcRect2, destRect2, null);

			return newBitmap;
		}

		private static List<Bitmap> GetShiftedBitmaps(Bitmap bitmap) {
			List<Bitmap> shiftedBitmaps = new List<Bitmap> ();
			int fragments = 10;
			int shiftLength = bitmap.Width / fragments;

			for (int i = 0; i < fragments; ++i) {
				shiftedBitmaps.Add (GetShiftedBitmap (bitmap, shiftLength * i));
			}

			return shiftedBitmaps;
		}

		private static AnimationDrawable GetAnimation(Bitmap bitmap) {
			AnimationDrawable animation = new AnimationDrawable ();
			animation.OneShot = (false);

			List<Bitmap> shiftedBitmaps = GetShiftedBitmaps (bitmap);
			int duration = 50;

			foreach (Bitmap image in shiftedBitmaps) {
				BitmapDrawable navigationBackground = new BitmapDrawable (MainActivityInstance.Current.Resources, image);
				navigationBackground.TileModeX = (global::Android.Graphics.Shader.TileMode.Repeat);

				animation.AddFrame (navigationBackground, duration);
			}
			return animation;
		}
            
		private static AlertDialog progressView;
	}
}

