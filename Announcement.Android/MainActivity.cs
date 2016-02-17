using Android.OS;
using Android.App;
using Android.Locations;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Content;
using Android.Views;
using Announcement.Core;
using Android.Widget;
using Android.Gms.Analytics;

namespace Announcement.Android
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize, Label = "StopSpam", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/splash_theme", WindowSoftInputMode = SoftInput.AdjustResize )]
    public class MainActivity : FragmentActivity, ViewTreeObserver.IOnGlobalLayoutListener
	{
        private MainViewModel ViewModel 
        { 
            get 
            { 
                return MainViewModel.Instance; 
            }
        }

		public Location lastKnownLocation;

		public LocationProvider locationProvider;

        protected override void OnCreate(Bundle bundle)
        {
            FontsManager.Load();
            
            base.OnCreate(bundle);

			SetTheme (Resource.Style.general_theme);

            SetContentView(Resource.Layout.main_layout);

			MainActivityInstance.Current = this;

            NavigationManager.Initialize(this);

            var mainContent = FindViewById<LinearLayout>(Resource.Id.MainContent);

            mainContent.ViewTreeObserver.AddOnGlobalLayoutListener(this);
        }
            
        public void OnGlobalLayout()
        {
            var mainContent = FindViewById<LinearLayout>(Resource.Id.MainContent);

            if (Build.VERSION.SdkInt < global::Android.OS.BuildVersionCodes.JellyBean) 
            {
                mainContent.ViewTreeObserver.RemoveGlobalOnLayoutListener(this); 
            } 
            else 
            {
                mainContent.ViewTreeObserver.RemoveOnGlobalLayoutListener(this); 
            }

            ViewModel.AutoLogin(AutoLoginSuccessCallback, AutoLoginFailCallback);
        }
            
        protected void AutoLoginSuccessCallback()
        {
            NavigationManager.AddHeader(typeof(HeaderFragment));
            
            switch (BaseViewModel.UserInfo.Role)
            {
                case UserRoles.User:
                    NavigationManager.Forward(typeof(UserMainFragment));
                    break;

                case UserRoles.Admin:
                    NavigationManager.Forward(typeof(AdminMainFragment));
                    break;

                case UserRoles.Moderator:
                    NavigationManager.Forward(typeof(ModeratorMainFragment));
                    break;
            }
        }

        protected void AutoLoginFailCallback()
        {
            NavigationManager.AddHeader(typeof(HeaderFragment));

            NavigationManager.Forward(typeof(LoginFragment));
        }

        public override void OnBackPressed()
        {
            if (!NavigationManager.Backward())
            {
                var main = new Intent(Intent.ActionMain);

                main.AddCategory(Intent.CategoryHome);

                main.SetFlags(ActivityFlags.NewTask);

                StartActivity(main);
            }
        }

		protected override void OnActivityResult (int requestCode, global::Android.App.Result resultCode, global::Android.Content.Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);

			SocialServices.Instance.OnActivityResult (requestCode, resultCode, data);
		}

		private static Tracker tracker;
		public static Tracker GATracker 
		{
			get {

				if (tracker == null) {
					var analytics = GoogleAnalytics.GetInstance (Application.Context);
					analytics.Logger.LogLevel = LoggerLogLevel.Verbose;
					tracker = analytics.NewTracker (GA_PROPERTY_ID);
					tracker.EnableAdvertisingIdCollection (true);				
				}

				return tracker;
			}
		}

		const string GA_PROPERTY_ID = "UA-73653423-1";
    }

	public class MainActivityInstance 
    {
		public static MainActivity Current
        {
			get 
            { 
                return current; 
            }
			set
            { 
                if (current == value)
                {
                    return;
                }
			
                if (value == null)
                {
                    Previous = current;
                }
			
				current = value;
            }
		}

		public static MainActivity Previous { get; set;}

		private static MainActivity current;
	}
}


