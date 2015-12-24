using System;
using Android.Util;
using Android.Runtime;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Content.Res;
using Android.Views.InputMethods;
using Android.App;
using Android.Widget;
using Announcement.Core;
using Android.OS;

namespace Announcement.Android
{
    public class LoginFragment : BaseFragment
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

			var loginEditText = view.FindViewById<TextView> (Resource.Id.LoginEditText);

			var passwordEditText = view.FindViewById<TextView> (Resource.Id.PasswordEditText);

            var loginButton = view.FindViewById<Button>(Resource.Id.LoginButton);

			var facebookButton = view.FindViewById<ImageView>(Resource.Id.FacebookButton);

			var googlePlusButton = view.FindViewById<ImageView>(Resource.Id.GooglePlusButton);

			var vkButton = view.FindViewById<ImageView>(Resource.Id.VKButton);

			var linkedInButton = view.FindViewById<ImageView>(Resource.Id.LinkedInButton);

			// mock data. need to delete
			loginEditText.Text = "username";
			passwordEditText.Text = "password";

            loginButton.Click += (sender, e) => 
                {
					ViewModel.LoginForAdminStuff(loginEditText.Text, passwordEditText.Text, LoginForAdminStuffCallback);
                };

            facebookButton.Click += (sender, e) =>
            {
                SocialServices.Instance.FacebookLogin((token) =>
                    { 
                            ViewModel.LoginViaFacebook(token, LoginViaSocialCallback);
                    });
            };

            googlePlusButton.Click += (sender, e) =>
            {
                SocialServices.Instance.GoogleLogin((token) =>
                    { 
                        ViewModel.LoginViaGooglePlus(token, LoginViaSocialCallback);
                    });
            };

            vkButton.Click += (sender, e) =>
            {
                SocialServices.Instance.VKLogin((token) =>
                    { 
                        ViewModel.LoginViaVK(token, LoginViaSocialCallback);
                    });
            };

            linkedInButton.Click += (sender, e) =>
            {
                SocialServices.Instance.LinkedInLogin((token) =>
                    { 
                        ViewModel.LoginViaLinkedIn(token, LoginViaSocialCallback);
                    });
            };

			NavigationManager.ShowHeader (false);

            view.SetOnTouchListener(this);

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


