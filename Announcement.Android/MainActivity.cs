using Android.OS;
using Android.App;
using Android.Locations;
using Android.Content.PM;
using Android.Support.V4.App;
using LocationProvider = Announcement.Android.Services.Location.LocationProvider;

namespace Announcement.Android
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, Label = "Announcement", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/splash_theme")]
	public class MainActivity : FragmentActivity, ILocationListener
	{
		public Location lastKnownLocation;
		private LocationProvider locationProvider;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

			SetTheme (Resource.Style.general_theme);

            SetContentView(Resource.Layout.main_layout);

			MainActivityInstance.Current = this;

            NavigationManager.Initialize(this);

			//NavigationManager.Forward(typeof(LoginFragment));
			NavigationManager.Forward(typeof(UserMainFragment));
			//NavigationManager.Forward(typeof(AdminMainFragment));

			locationProvider = new LocationProvider (this, this);
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

		protected override void OnResume ()
		{
			base.OnResume ();
			if(locationProvider!=null)
				locationProvider.OnResume ();
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			if(locationProvider!=null)
				locationProvider.OnPause ();
		}

		#region ILocationListener implementation

		public void OnLocationChanged (Location location)
		{
			if (location == null)
				return;

			lastKnownLocation = location;
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
		}

		#endregion
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


