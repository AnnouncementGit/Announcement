using System;
using Xamarin.Auth;
using Android.App;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;

namespace Announcement.Android
{
	public class SocialServices : Android.Gms.Common.Apis.GoogleApiClient.IConnectionCallbacks
	{
		private MainActivity activity;

		private static SocialServices instance;
		public static SocialServices Instance
		{
			get{ return instance!= null ? instance : (instance = new SocialServices());}
		}

		private SocialServices ()
		{
			activity = MainActivityInstance.Current;
		}

		#region FacebookLogin SDK - get token = ok
		bool isFacebookInit;
		public static ICallbackManager callbackManager;
		public void FacebookLogin(Action<string> callback)
		{
			if (activity != null)
			{
				if (!isFacebookInit)
				{
					FacebookSdk.SdkInitialize(activity.ApplicationContext);

					isFacebookInit = true;

					callbackManager = CallbackManagerFactory.Create();

					var accesstokenTracker = new FacebookAcessTokenTracker();

					accesstokenTracker.StartTracking();

					accesstokenTracker.AccessTokenChanged += (sender, e) =>
					{
						if(callback != null &&!string.IsNullOrWhiteSpace(e.Token))
							callback.Invoke(e.Token);
					};
					LoginManager.Instance.RegisterCallback (callbackManager, null);
				}
				LoginManager.Instance.LogInWithReadPermissions (activity, new [] { "user_friends" });
			}
		}
		#endregion
		#region FacebookLogin oAuth2 - get token = ok
		//		public void FacebookLogin(Action<string> callback){
		//			var auth = new OAuth2Authenticator (
		//				clientId: "467795070094543",
		//				scope: "",
		//				authorizeUrl: new Uri ("https://m.facebook.com/dialog/oauth/"),
		//				redirectUrl: new Uri ("http://www.facebook.com/connect/login_success.html"));
		//
		//			auth.Completed += (s, e) => {
		//				if (!e.IsAuthenticated) 
		//					ShowErrorMessage("Not Authenticated");
		//				else{
		//					var token = e.Account.Properties ["access_token"].ToString ();
		//					if(callback != null && !string.IsNullOrWhiteSpace(token))
		//						callback.Invoke(token);
		//				}
		//			} ;
		//
		//			var intent = auth.GetUI (activity);
		//			activity.StartActivity (intent);
		//		}
		#endregion
		#region GoogleLogin SDK - get token = 

		private bool googleApiIntentInProgress;
		private bool googleApiSignInClicked;
		private GoogleApiClient googleApiClient;
		private ConnectionResult googleConnectionResult;

		public void GoogleLogin(Action<string> callback)
		{
			if (googleConnectionResult.HasResolution) {
				try {
				
					googleApiIntentInProgress = true;
					MainActivityInstance.Current.StartIntentSenderForResult (googleConnectionResult.Resolution.IntentSender, 0, null, 0, 0, 0);
				} catch (global::Android.Content.IntentSender.SendIntentException exception) {
					googleApiIntentInProgress = false;
					googleApiClient.Connect ();
					Console.WriteLine (exception.Message);
				}
			} else {
				GooglePlayServicesUtil.GetErrorDialog (googleConnectionResult.ErrorCode, activity, 0).Show ();
			}
		}

		private void AddGoogleApiClient()
		{
			var googleApiClientBuilder = new GoogleApiClient.Builder (activity);
			googleApiClientBuilder.AddConnectionCallbacks (this);
			googleApiClientBuilder.AddOnConnectionFailedListener (this);
			googleApiClientBuilder.AddApi (PlusClass.API);
			googleApiClientBuilder.AddScope (PlusClass.ScopePlusProfile);
			googleApiClientBuilder.AddScope (PlusClass.ScopePlusLogin);
			googleApiClient = googleApiClientBuilder.Build ();
		}
		#endregion
		#region GoogleLogin oAuth2 - get token = bad
//		public void GoogleLogin(Action<string> callback)
//		{
//			OAuth2Authenticator auth = new OAuth2Authenticator (
//				                           clientId: "1096133023554-5lphn591aukp6il8047l8r89km4cedlv.apps.googleusercontent.com",
//				                           clientSecret: "MuFdnTnQAsA0q4ufRk6lpcn",
//				                           scope: "https://www.googleapis.com/auth/plus.login",
//				                           authorizeUrl: new Uri ("https://accounts.google.com/o/oauth2/auth"),
//				                           redirectUrl: new Uri ("https://localhost/oauth2callback"),
//				                           accessTokenUrl: new Uri ("https://accounts.google.com/o/oauth2/token"),
//				                           getUsernameAsync: null) 
//			{
//				AllowCancel = true,
//				ShowUIErrors = false,
//				Title = "Google Plus Login"
//			};
//
//			auth.Completed += (s, e) => {
//				if (!e.IsAuthenticated)
//					ShowErrorMessage ("Not Authenticated");
//				else {
//					var token = e.Account.Properties ["access_token"].ToString ();
//					if (callback != null && !string.IsNullOrWhiteSpace (token))
//						callback.Invoke (token);
//				}
//			};
//
//			var intent = auth.GetUI (activity);
//			activity.StartActivity (intent);
//		}
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

		public void OnActivityResult(int requestCode, Result resultCode, global::Android.Content.Intent data)
		{
			if (requestCode == 0) {
				if (resultCode != Result.Ok)
					googleApiSignInClicked = false;
				
				googleApiIntentInProgress = false;
				
				if (!googleApiClient.IsConnecting)
					googleApiClient.Connect ();
			}

			if (callbackManager != null)
				callbackManager.OnActivityResult (requestCode, (int)resultCode, data);
			
		}
	}

	public class FacebookAcessTokenTracker : AccessTokenTracker
	{
		public event EventHandler<Xamarin.Facebook.AccessToken> AccessTokenChanged;

		public FacebookAcessTokenTracker() : base()
		{

		}

		protected override void OnCurrentAccessTokenChanged(Xamarin.Facebook.AccessToken oldAcessToken, Xamarin.Facebook.AccessToken currentAcessToken)
		{
			if (currentAcessToken != null)
				Console.WriteLine ("Facebook access token:" +currentAcessToken.Token);

			var handler = AccessTokenChanged;

			if (handler != null)
			{
				handler(this, currentAcessToken);
			}
		}
	}
}

