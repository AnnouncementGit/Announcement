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

			var facebookButton = view.FindViewById<ImageView>(Resource.Id.FacebookButton);

			var googlePlusButton = view.FindViewById<ImageView>(Resource.Id.GooglePlusButton);

			var vkButton = view.FindViewById<ImageView>(Resource.Id.VKButton);

			var linkedInButton = view.FindViewById<ImageView>(Resource.Id.LinkedInButton);

			//var moderatorButton = view.FindViewById<ImageView>(Resource.Id.ModeratorButton);

            loginButton.Click += (sender, e) => 
                {
                    ViewModel.LoginForAdminStuff("username", "password", LoginForAdminStuffCallback);
                };

            facebookButton.Click += (sender, e) =>
            {
                SocialServices.Instance.FacebookLogin((token) =>
                    { 
                        ViewModel.LoginViaFacebook("", LoginViaSocialCallback);
                    });
            };

			googlePlusButton.Click += (sender, e) => 
			{
				SocialServices.Instance.GoogleLogin((token) => { 
					ViewModel.LoginViaGooglePlus(token, LoginViaSocialCallback);
				});
			};

			vkButton.Click += (sender, e) => 
			{
				SocialServices.Instance.VKLogin((token) => { 
					ViewModel.LoginViaVK(token, LoginViaSocialCallback);
				});
			};

			linkedInButton.Click += (sender, e) => 
			{
				SocialServices.Instance.LinkedInLogin((token) => { 
					ViewModel.LoginViaLinkedIn(token, LoginViaSocialCallback);
				});
			};

			NavigationManager.ShowHeader (false);

            return view;
        }

        protected void LoginForAdminStuffCallback()
        {
            NavigationManager.EraseBackStack();

            NavigationManager.Forward(typeof(AdminMainFragment));
        }

		protected void LoginViaSocialCallback()
		{
			NavigationManager.EraseBackStack();

			NavigationManager.Forward(typeof(UserMainFragment));
		}
    }
}


