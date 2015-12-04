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
            var view = inflater.Inflate(Resource.Layout.admin_main_layout, null);

            var moderatorsButton = view.FindViewById<Button>(Resource.Id.ModeratorsButton);

            var validationsButton = view.FindViewById<Button>(Resource.Id.ValidationsButton);

            moderatorsButton.Click += (sender, e) => 
                {
                    //ViewModel.InitializeModerators(InitializeModeratorsCallback);
                };

            validationsButton.Click += (sender, e) => 
                {
                    //ViewModel.InitializeValidations(InitializeValidationsCallback);
                };

            return view;
        }

        public void InitializeModeratorsCallback()
        {
            //NavigationManager.Forward(typeof(ModeratorsFragment));
        }

        public void InitializeValidationsCallback()
        {
            //NavigationManager.Forward(typeof(ModeratorsFragment));
        }
    }
}


