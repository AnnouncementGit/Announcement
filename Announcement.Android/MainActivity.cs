using Android.OS;
using Android.App;
using Android.Support.V4.App;

namespace Announcement.Android
{
    [Activity(Label = "Announcement", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/splash_theme", ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : FragmentActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.main_layout);

			MainActivityInstance.Current = this;

            NavigationManager.Initialize(this);

            NavigationManager.Forward(typeof(LoginViaSocialFragment));
        }

        public override void OnBackPressed()
        {
            if (!NavigationManager.Backward())
            {
                base.OnBackPressed();
            }
        }

		protected override void OnActivityResult (int requestCode, Result resultCode, global::Android.Content.Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			SocialServices.Instance.OnActivityResult (requestCode, resultCode, data);
		}
    }

	public class MainActivityInstance {
		public static MainActivity Current{
			get { return current; }
			set{ if (current == value)
					return;
			
				if (value == null)
					Previous = current;
			
				current = value;}
		}
		public static MainActivity Previous { get; set;}
		private static MainActivity current;
	}
}


