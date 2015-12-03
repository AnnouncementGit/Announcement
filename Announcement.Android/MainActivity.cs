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
    }
}


