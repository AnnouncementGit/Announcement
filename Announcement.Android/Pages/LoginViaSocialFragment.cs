using Android.OS;
using Android.Views;
using Android.Widget;
using Announcement.Core;
using Android.Support.V4.App;

namespace Announcement.Android
{
    public class LoginViaSocialFragment : Fragment
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
            var view = inflater.Inflate(Resource.Layout.login_via_social_layout, null);

            var facebookButton = view.FindViewById<Button>(Resource.Id.FacebookButton);

            var googlePlusButton = view.FindViewById<Button>(Resource.Id.GooglePlusButton);

            var vkButton = view.FindViewById<Button>(Resource.Id.VKButton);

            var linkedInButton = view.FindViewById<Button>(Resource.Id.LinkedInButton);

            var moderatorButton = view.FindViewById<Button>(Resource.Id.ModeratorButton);

            facebookButton.Click += (sender, e) => 
            {
				SocialServices.Instance.FacebookLogin((token) => { 
					ViewModel.LoginViaFacebook(token, LoginViaSocialCallback);
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

            moderatorButton.Click += (sender, e) => 
                {
                    NavigationManager.Forward(typeof(LoginFragment));
                };

            return view;
        }

        protected void LoginViaSocialCallback()
        {
            NavigationManager.EraseBackStack();
            
            NavigationManager.Forward(typeof(UserMainFragment));
        }
    }
}


