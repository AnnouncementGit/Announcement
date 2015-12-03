using Android.OS;
using Android.Views;
using Android.Widget;
using Announcement.Core;
using Android.Support.V4.App;

namespace Announcement.Android
{
    public class LoginFragment : Fragment
    {
        private LoginViewModel ViewModel 
        { 
            get 
            { 
                return LoginViewModel.Instance; 
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.login_layout, null);

            var loginButton = view.FindViewById<Button>(Resource.Id.LoginButton);

            loginButton.Click += (sender, e) => 
                {
                    ViewModel.LoginForAdminStuff("username", "password", LoginForAdminStuffCallback);
                };

            return view;
        }

        protected void LoginForAdminStuffCallback()
        {
            NavigationManager.EraseBackStack();

            NavigationManager.Forward(typeof(AdminMainFragment));
        }
    }
}


