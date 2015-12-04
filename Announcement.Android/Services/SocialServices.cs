using System;
using Xamarin.Auth;
using Android.App;

namespace Announcement.Android
{
	public class SocialServices
	{
		private MainActivity activity;
		public SocialServices ()
		{
			activity = MainActivityInstance.Current;
		}

		#region GoogleLogin - get token = bad
		public void GoogleLogin(Action<string> callback)
		{
			OAuth2Authenticator auth = new OAuth2Authenticator (
				                           clientId: "1096133023554-5lphn591aukp6il8047l8r89km4cedlv.apps.googleusercontent.com",
				                           clientSecret: "MuFdnTnQAsA0q4ufRk6lpcn",
				                           scope: "https://www.googleapis.com/auth/plus.login",
				                           authorizeUrl: new Uri ("https://accounts.google.com/o/oauth2/auth"),
				                           redirectUrl: new Uri ("https://localhost/oauth2callback"),
				                           accessTokenUrl: new Uri ("https://accounts.google.com/o/oauth2/token"),
				                           getUsernameAsync: null) 
			{
				AllowCancel = true,
				ShowUIErrors = false,
				Title = "Google Plus Login"
			};

			auth.Completed += (s, e) => {
				if (!e.IsAuthenticated)
					ShowErrorMessage ("Not Authenticated");
				else {
					var token = e.Account.Properties ["access_token"].ToString ();
					if (callback != null && !string.IsNullOrWhiteSpace (token))
						callback.Invoke (token);
				}
			};

			var intent = auth.GetUI (activity);
			activity.StartActivity (intent);
		}
		#endregion
		#region FacebookLogin - get token = ok
		public void FacebookLogin(Action<string> callback){
			var auth = new OAuth2Authenticator (
				clientId: "467795070094543",
				scope: "",
				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));

			auth.Completed += (s, e) => {
				if (!e.IsAuthenticated) 
					ShowErrorMessage("Not Authenticated");
				else{
					var token = e.Account.Properties ["access_token"].ToString ();
					if(callback != null && !string.IsNullOrWhiteSpace(token))
						callback.Invoke(token);
				}
			} ;

			var intent = auth.GetUI (activity);
			activity.StartActivity (intent);
		}

		#endregion
		#region LinkedInLogin - get token = ok

		public void LinkedInLogin(Action<string> callback)
		{
			var auth = new OAuth2Authenticator (
				clientId: "77o5jxdgmf5uho",
				clientSecret: "XCYQtNLFRUoMGPVO",
				scope: "",
				authorizeUrl: new Uri ("https://www.linkedin.com/uas/oauth2/authorization"),
				redirectUrl: new Uri ("https://oauth.vk.com/blank.html"),  //need to change
				accessTokenUrl: new Uri ("https://www.linkedin.com/uas/oauth2/accessToken")
			);
			auth.AllowCancel = true;
			auth.Completed += (s, e) => {
				if (!e.IsAuthenticated) 
					ShowErrorMessage("Not Authenticated");
				else
				{
					var token = e.Account.Properties ["access_token"].ToString ();
					if(callback != null && !string.IsNullOrWhiteSpace(token))
						callback.Invoke(token);
				}
			} ;

			var intent = auth.GetUI (activity);
			activity.StartActivity (intent);
		}

		#endregion
		#region VKLogin - get token = ok
		public void VKLogin (Action<string> callback)
		{
			var auth = new OAuth2Authenticator (
				clientId: "5173092",
				scope: "friends,video,groups",
				authorizeUrl: new Uri ("https://oauth.vk.com/authorize"),
				redirectUrl: new Uri ("https://oauth.vk.com/blank.html"));
			auth.AllowCancel = true;
			auth.Completed += (s, e) => {
				if (!e.IsAuthenticated)
					ShowErrorMessage("Not Authenticated");
				else
				{
					var token = e.Account.Properties ["access_token"].ToString ();
					if(callback!=null && !string.IsNullOrWhiteSpace(token))
						callback.Invoke(token);   
				}
			} ;

			var intent = auth.GetUI (activity);
			activity.StartActivity (intent);
		}

		private void ShowErrorMessage(string message)
		{
			var builder = new AlertDialog.Builder (activity);
			builder.SetMessage (message);
			builder.SetPositiveButton ("Ok", (o, eee)=>{});
			builder.Create().Show();
		}

		#endregion
	}
}

