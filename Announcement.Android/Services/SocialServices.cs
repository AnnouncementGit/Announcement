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
using Announcement.Core;

namespace Announcement.Android
{
    public class SocialServices : Java.Lang.Object, global::Android.Gms.Common.Apis.GoogleApiClient.IConnectionCallbacks, global::Android.Gms.Common.Apis.GoogleApiClient.IOnConnectionFailedListener
	{
		public static SocialServices Instance
        {
            get
            { 
                return instance != null ? instance : (instance = new SocialServices());
            }
        }

		private SocialServices ()
		{
			activity = MainActivityInstance.Current;
		}

		#region FacebookLogin SDK - get token = ok
		
		public void FacebookLogin(Action<string> callback)
		{
			if (isSocailServiceInProcess) {
				return;
			}
            
			if (activity != null) {
				isSocailServiceInProcess = true;

				ProgressModule.Message (LocalizationModule.Translate ("progress_authentication"), false);
                
				if (!FacebookSdk.IsInitialized) {
					FacebookSdk.SdkInitialize (activity.ApplicationContext);

					if (callback != null && AccessToken.CurrentAccessToken != null && !string.IsNullOrWhiteSpace (AccessToken.CurrentAccessToken.Token)) {
						isSocailServiceInProcess = false;
						callback.Invoke (AccessToken.CurrentAccessToken.Token);

						return;
					}

					var facebookCallback = new FacebookCallback ();

					facebookCallback.Cancel += (sender, e) => {
						isSocailServiceInProcess = false; 

						ProgressModule.End ();
					};

					facebookCallback.Error += (sender, e) => {
						isSocailServiceInProcess = false;

						ProgressModule.End ();
                            
						AlertModule.Show (LocalizationModule.Translate ("alert_title_facebook"), LocalizationModule.Translate ("alert_no_internet_connection"), LocalizationModule.Translate ("alert_button_ok"));
					};

					facebookCallback.Success += (sender, e) => {
						isSocailServiceInProcess = false;

						if (callback != null && AccessToken.CurrentAccessToken!=null && !string.IsNullOrWhiteSpace (AccessToken.CurrentAccessToken.Token)) {
							callback.Invoke (AccessToken.CurrentAccessToken.Token);
						} else
							ProgressModule.End ();
					};

					callbackManager = CallbackManagerFactory.Create ();

					LoginManager.Instance.RegisterCallback (callbackManager, facebookCallback);
				}
				LoginManager.Instance.LogInWithReadPermissions (activity, new [] { "user_friends" });
			}
		}
		#endregion

		#region GoogleLogin SDK - get token = ok

		private GoogleApiClient googleApiClient;
		private ConnectionResult googleConnectionResult;
		private bool googleLogInButtonClicked;
		private Action<string> googleLoginCallback;
		public const int GoogleRecoverableAuthRequestCode = 111;

		public void GoogleLogin(Action<string> callback)
		{
            if (isSocailServiceInProcess)
            {
                return;
            }

			isSocailServiceInProcess = true;

			ProgressModule.Message(LocalizationModule.Translate("progress_authentication"), false);

			googleLogInButtonClicked = true;

			AddGoogleApiClient ();

			googleApiClient.Connect ();

            if (callback != null)
            {
                googleLoginCallback = callback;
            }
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

        public void OnConnectionSuspended(int cause)
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
			} else
				ProgressModule.End ();
		}

		public void OnConnected (global::Android.OS.Bundle connectionHint)
		{
			var account = PlusClass.AccountApi.GetAccountName (googleApiClient);

			Task.Run (() => {
				var token = string.Empty;
				try
				{
					ProgressModule.Message(LocalizationModule.Translate("progress_authentication"), false);
					token = GoogleAuthUtil.GetToken(MainActivityInstance.Current, account,"oauth2:" + Scopes.PlusLogin + " https://www.googleapis.com/auth/plus.profile.emails.read");

					if (!string.IsNullOrWhiteSpace(token) && googleLoginCallback != null){
						googleLoginCallback.Invoke (token);
						isSocailServiceInProcess = false;
					}
				}
				catch (UserRecoverableAuthException e)
				{
					MainActivityInstance.Current.StartActivityForResult (e.Intent, SocialServices.GoogleRecoverableAuthRequestCode);
				}
				catch (GoogleAuthException ex){
					var t = ex.Message;

					isSocailServiceInProcess = false;

					ProgressModule.End();
				}
			});
		}
		#endregion
		#region LinkedInLogin - get token = ok

		public void LinkedInLogin(Action<string> callback)
		{
			if (isSocailServiceInProcess)
            {
                return;
            }

			isSocailServiceInProcess = true;

			ProgressModule.Message(LocalizationModule.Translate("progress_authentication"), false);
            
			var auth = new OAuth2Authenticator (
				clientId: "77o5jxdgmf5uho",
				clientSecret: "XCYQtNLFRUoMGPVO",
				scope: "",
				authorizeUrl: new Uri ("https://www.linkedin.com/uas/oauth2/authorization"),
				redirectUrl: new Uri ("https://oauth.vk.com/blank.html"),  //need to change
				accessTokenUrl: new Uri ("https://www.linkedin.com/uas/oauth2/accessToken")
			);
			auth.AllowCancel = true;
			auth.ShowUIErrors = false;
			auth.Completed += (s, e) => {
				if (!e.IsAuthenticated) {
					AlertModule.ShowInformation("Not Authenticated", null);
					ProgressModule.End();
				}
				else
				{
					var token = e.Account.Properties ["access_token"].ToString ();
					if(callback != null && !string.IsNullOrWhiteSpace(token))
						callback.Invoke(token);
				}

				isSocailServiceInProcess = false;
			} ;

			var intent = auth.GetUI (activity);
			activity.StartActivity (intent);
		}

		#endregion
		#region VKLogin - get token = ok
		public void VKLogin (Action<string> callback)
		{
			if (isSocailServiceInProcess)
            {
                return;
            }

			isSocailServiceInProcess = true;

			ProgressModule.Message(LocalizationModule.Translate("progress_authentication"), false);
            
			var auth = new OAuth2Authenticator (
				clientId: "5173092",
				scope: "friends,video,groups",
				authorizeUrl: new Uri ("https://oauth.vk.com/authorize"),
				redirectUrl: new Uri ("https://oauth.vk.com/blank.html"));
			auth.AllowCancel = true;
			auth.ShowUIErrors = false;
			auth.Completed += (s, e) => {
				if (!e.IsAuthenticated){
					AlertModule.ShowInformation("Not Authenticated", null);
					ProgressModule.End();
				}
				else
				{
					var token = e.Account.Properties ["access_token"].ToString ();
					if(callback!=null && !string.IsNullOrWhiteSpace(token))
						callback.Invoke(token);   
				}

				isSocailServiceInProcess = false;
			} ;

			var intent = auth.GetUI (activity);
			activity.StartActivity (intent);
		}

		#endregion

		public void OnActivityResult(int requestCode, global::Android.App.Result resultCode, global::Android.Content.Intent data)
		{
			isSocailServiceInProcess = false;

			if (googleApiClient != null && !googleApiClient.IsConnecting && requestCode == 0) {
				googleApiClient.Connect ();
			}

			if (requestCode == GoogleRecoverableAuthRequestCode) 
			{
				var extras = data.Extras;
				var token = extras.GetString ("authtoken");
                if (!string.IsNullOrWhiteSpace(token) && googleLoginCallback != null)
                {
					googleLoginCallback.Invoke(token);
                }
				else
					ProgressModule.End ();
			}

            if (FacebookSdk.IsInitialized && requestCode == FacebookSdk.CallbackRequestCodeOffset && callbackManager != null)
            {
				callbackManager.OnActivityResult (requestCode, (int)resultCode, data);
            }
		}

		public void LogOut()
		{
			LoginManager.Instance.LogOut ();
			
			if (googleApiClient != null && googleApiClient.IsConnected)
				PlusClass.AccountApi.RevokeAccessAndDisconnect (googleApiClient);
		}

        private MainActivity activity;

        private static SocialServices instance;

        public static ICallbackManager callbackManager;

		private bool isSocailServiceInProcess;
	}

    public class FacebookCallback : Java.Lang.Object, IFacebookCallback
    {
        public event EventHandler Cancel;

        public event EventHandler<FacebookException> Error;

        public event EventHandler<Java.Lang.Object> Success;

        public void OnCancel()
        {
            var handler = Cancel;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }  
        }

        public void OnError(FacebookException exception)
        {
            var handler = Error;

            if (handler != null)
            {
                handler(this, exception);
            }
        }

        public void OnSuccess(Java.Lang.Object value)
        {
            var handler = Success;

            if (handler != null)
            {
                handler(this, value);
            }
        }
    }
}