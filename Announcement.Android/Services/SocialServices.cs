using System;
using Xamarin.Auth;
using Android.App;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.Gms.Auth;
using System.Threading.Tasks;
using Android.OS;

namespace Announcement.Android
{
	public class SocialServices : Java.Lang.Object, global::Android.Gms.Common.Apis.GoogleApiClient.IConnectionCallbacks, global::Android.Gms.Common.Apis.GoogleApiClient.IOnConnectionFailedListener
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
		#region GoogleLogin SDK - get token = ok

		private GoogleApiClient googleApiClient;
		private ConnectionResult googleConnectionResult;
		private bool googleLogInButtonClicked;
		private Action<string> googleLoginCallback;

		public void GoogleLogin(Action<string> callback)
		{
			googleLogInButtonClicked = true;
			AddGoogleApiClient ();
			googleApiClient.Connect ();

			if(callback!=null)
			    googleLoginCallback = callback;
		}

		private void AddGoogleApiClient()
		{
			var googleApiClientBuilder = new GoogleApiClient.Builder (activity);
			googleApiClientBuilder.AddConnectionCallbacks (this);
			googleApiClientBuilder.AddOnConnectionFailedListener (this);
			googleApiClientBuilder.AddApi (PlusClass.API);
			googleApiClientBuilder.AddScope (PlusClass.ScopePlusLogin);
			googleApiClientBuilder.AddScope (PlusClass.ScopePlusProfile);
			googleApiClient = googleApiClientBuilder.Build ();
		}

		public void OnConnectionSuspended (int cause)
		{
		}

		public void OnConnectionFailed (ConnectionResult result)
		{
			googleConnectionResult = result;

			if (googleLogInButtonClicked) {
				googleLogInButtonClicked = false;
				if (googleConnectionResult.HasResolution) {
					try {
						MainActivityInstance.Current.StartIntentSenderForResult (googleConnectionResult.Resolution.IntentSender, 0, null, 0, 0, 0);
					} catch (global::Android.Content.IntentSender.SendIntentException exception) {
						googleApiClient.Connect ();
						Console.WriteLine (exception.Message);
					}
				} else {
					GooglePlayServicesUtil.GetErrorDialog (googleConnectionResult.ErrorCode, activity, 0).Show ();
				}
			}
		}

		public void OnConnected (global::Android.OS.Bundle connectionHint)
		{
//			if (PlusClass.PeopleApi.GetCurrentPerson (googleApiClient) != null) {
//				var person = PlusClass.PeopleApi.GetCurrentPerson (googleApiClient);
//			}
//
			var account = PlusClass.AccountApi.GetAccountName (googleApiClient);

			var t = new GoogleAccessTokenAsyncLoader (account, googleLoginCallback);
			t.Execute ();
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
			if (googleApiClient != null && !googleApiClient.IsConnecting && requestCode == 0)
				googleApiClient.Connect ();

			if (callbackManager != null)
				callbackManager.OnActivityResult (requestCode, (int)resultCode, data);
		}
	}

	public class GoogleAccessTokenAsyncLoader : AsyncTask<string, string, string>
	{
		string account;
		Action<string> callback;
		public GoogleAccessTokenAsyncLoader(string account, Action<string> callback)
		{
			this.account = account;
			this.callback = callback;
		}
		#region implemented abstract members of AsyncTask

		protected override string RunInBackground (params string[] @params)
		{
			var token = string.Empty;
			try
			{
				token = GoogleAuthUtil.GetToken(MainActivityInstance.Current, account,"oauth2:" + Scopes.PlusLogin);

				if (callback != null)
					callback.Invoke (token);
			}
			catch (Exception){
				//
			}

			return token;
		}
		#endregion
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

