using Android.OS;
using Android.Views;
using Android.Widget;
using Announcement.Core;
using Android.Support.V4.App;

namespace Announcement.Android
{
    public class AdminMainFragment : Fragment
    {
        private AdminMainViewModel ViewModel 
        { 
            get 
            { 
                return AdminMainViewModel.Instance; 
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.super_admin_main_layout, null);

            var moderatorsButton = view.FindViewById<Button>(Resource.Id.ModeratorsButton);

            moderatorsButton.Click += (sender, e) => 
                {
                    ViewModel.OpenModerators(OpenModeratorsCallback);
                };

            return view;
        }

        public void OpenModeratorsCallback()
        {
            //NavigationManager.Forward(typeof(ModeratorsFragment));
        }
    }
}


