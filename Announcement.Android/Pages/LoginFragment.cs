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
using Android.Gms.Analytics;
using System.Net.Http;
using PCLStorage;

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


            loginButton.Click += (sender, e) => 
                {
					ViewModel.LoginForAdminStuff(loginEditText.Text, passwordEditText.Text, LoginForAdminStuffCallback);
                };

            facebookButton.Click += (sender, e) =>
            {
				SocialServices.Instance.FacebookLogin((userId, username, token) =>
                    { 
						ViewModel.LoginViaFacebook(userId, username, token, LoginViaSocialCallback);
                    });
            };

            googlePlusButton.Click += (sender, e) =>
            {
				SocialServices.Instance.GoogleLogin((userId, username, token) =>
                    { 
						ViewModel.LoginViaGooglePlus(userId, username, token, LoginViaSocialCallback);
                    });
            };

            vkButton.Click += (sender, e) =>
            {
				SocialServices.Instance.VKLogin((userId, username, token) =>
                    { 
						ViewModel.LoginViaVK(userId, username, token, LoginViaSocialCallback);
                    });
            };

            linkedInButton.Click += (sender, e) =>
            {
				SocialServices.Instance.LinkedInLogin((userId, username, token) =>
                    { 
						ViewModel.LoginViaLinkedIn(userId, username, token, LoginViaSocialCallback);
                    });
            };

			NavigationManager.ShowHeader (false);

            view.SetOnTouchListener(this);

			MainActivity.GATracker.SetScreenName ("Login Fragment");
			MainActivity.GATracker.Send (new HitBuilders.ScreenViewBuilder ().Build ());
			OpenFile ();
            return view;
        }


		async void OpenFile()
		{
			try
			{
				using (var httpClient = new HttpClient())
				{
					var stream = await httpClient.GetStreamAsync("https://docs.google.com/spreadsheets/d/19kevGGgTl9R3cnvZgQ4K5z6lnRBOq3WGfQpch_-hTAM/edit?usp=sharing");
				}


			}
			catch (Exception ex)
			{
				var temp = ex.Message;
			}
		}

        protected void LoginForAdminStuffCallback()
        {
            NavigationManager.EraseBackStack();

            if (BaseViewModel.UserInfo != null)
            {
                if (BaseViewModel.UserInfo.Role == UserRoles.Admin)
                {
                    NavigationManager.Forward(typeof(AdminMainFragment));
                }
                else
                {
                    NavigationManager.Forward(typeof(ModeratorMainFragment));
                }
            }
        }

		protected void LoginViaSocialCallback()
        {
            NavigationManager.EraseBackStack();

            NavigationManager.Forward(typeof(UserMainFragment));
        }
    }
}


