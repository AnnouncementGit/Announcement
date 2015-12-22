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
            if (isFacebookInProcess)
            {
                return;
            }
            
			if (activity != null)
			{
                isFacebookInProcess = true;
                
                if (!FacebookSdk.IsInitialized)
				{
					FacebookSdk.SdkInitialize(activity.ApplicationContext);

					var accesstokenTracker = new FacebookAcessTokenTracker();

					accesstokenTracker.StartTracking();

                    accesstokenTracker.AccessTokenChanged += (sender, e) =>
                    {
                        isFacebookInProcess = false;
                            
                        if (callback != null && !string.IsNullOrWhiteSpace(e.Token))
                        {
                            callback.Invoke(e.Token);
                        }
                    };

                    var facebookCallback = new FacebookCallback();

                    facebookCallback.Cancel += (sender, e) => 
                        {
                            isFacebookInProcess = false; 
                        };

                    facebookCallback.Error += (sender, e) => 
                        {
                            isFacebookInProcess = false;
                            
                            AlertModule.Show(LocalizationModule.Translate("alert_title_facebook"), LocalizationModule.Translate("alert_no_internet_connection"), LocalizationModule.Translate("alert_button_ok"));
                        };

                    facebookCallback.Success += (sender, e) => 
                        {
                            isFacebookInProcess = false;
                        };

                    callbackManager = CallbackManagerFactory.Create();

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
            if (isGooglePlusInProcess)
            {
                return;
            }

            isGooglePlusInProcess = true;

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
			}
		}

		public void OnConnected (global::Android.OS.Bundle connectionHint)
		{
			var account = PlusClass.AccountApi.GetAccountName (googleApiClient);

			Task.Run (() => {
				var token = string.Empty;
				try
				{
					token = GoogleAuthUtil.GetToken(MainActivityInstance.Current, account,"oauth2:" + Scopes.PlusLogin + " https://www.googleapis.com/auth/plus.profile.emails.read");

					isGooglePlusInProcess = false;

					if (!string.IsNullOrWhiteSpace(token) && googleLoginCallback != null)
						googleLoginCallback.Invoke (token);
				}
				catch (UserRecoverableAuthException e)
				{
					MainActivityInstance.Current.StartActivityForResult (e.Intent, SocialServices.GoogleRecoverableAuthRequestCode);
				}
				catch (GoogleAuthException ex){
					var t = ex.Message;

					isGooglePlusInProcess = false;
				}
			});
		}
		#endregion
		#region LinkedInLogin - get token = ok

		public void LinkedInLogin(Action<string> callback)
		{
            if (isLinkeInInProcess)
            {
                return;
            }

            isLinkeInInProcess = true;
            
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
				if (!e.IsAuthenticated) 
					AlertModule.ShowInformation("Not Authenticated", null);
				else
				{
					var token = e.Account.Properties ["access_token"].ToString ();
					if(callback != null && !string.IsNullOrWhiteSpace(token))
						callback.Invoke(token);
				}

                isLinkeInInProcess = false;
			} ;

			var intent = auth.GetUI (activity);
			activity.StartActivity (intent);
		}

		#endregion
		#region VKLogin - get token = ok
		public void VKLogin (Action<string> callback)
		{
            if (isVKInProcess)
            {
                return;
            }

            isVKInProcess = true;
            
			var auth = new OAuth2Authenticator (
				clientId: "5173092",
				scope: "friends,video,groups",
				authorizeUrl: new Uri ("https://oauth.vk.com/authorize"),
				redirectUrl: new Uri ("https://oauth.vk.com/blank.html"));
			auth.AllowCancel = true;
			auth.ShowUIErrors = false;
			auth.Completed += (s, e) => {
				if (!e.IsAuthenticated)
					AlertModule.ShowInformation("Not Authenticated", null);
				else
				{
					var token = e.Account.Properties ["access_token"].ToString ();
					if(callback!=null && !string.IsNullOrWhiteSpace(token))
						callback.Invoke(token);   
				}

                isVKInProcess = false;
			} ;

			var intent = auth.GetUI (activity);
			activity.StartActivity (intent);
		}

		#endregion

		public void OnActivityResult(int requestCode, global::Android.App.Result resultCode, global::Android.Content.Intent data)
		{
			if (googleApiClient != null && !googleApiClient.IsConnecting && requestCode == 0) {
				googleApiClient.Connect ();
			}

			if (googleApiClient == null || !googleApiClient.IsConnecting){
				isGooglePlusInProcess = false;
			}

			if (requestCode == GoogleRecoverableAuthRequestCode) 
			{
				var extras = data.Extras;
				var token = extras.GetString ("authtoken");
                if (!string.IsNullOrWhiteSpace(token) && googleLoginCallback != null)
                {
					googleLoginCallback.Invoke(token);
					isGooglePlusInProcess = false;
                }
			}

            if (FacebookSdk.IsInitialized && requestCode == FacebookSdk.CallbackRequestCodeOffset && callbackManager != null)
            {
				callbackManager.OnActivityResult (requestCode, (int)resultCode, data);
            }
		}

        private MainActivity activity;

        private static SocialServices instance;

        public static ICallbackManager callbackManager;

        private bool isFacebookInProcess;

        private bool isGooglePlusInProcess;

        private bool isVKInProcess;

        private bool isLinkeInInProcess;
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

	public class FacebookAcessTokenTracker : AccessTokenTracker
	{
		public event EventHandler<Xamarin.Facebook.AccessToken> AccessTokenChanged;

		public FacebookAcessTokenTracker() : base()
		{

		}

		protected override void OnCurrentAccessTokenChanged(Xamarin.Facebook.AccessToken oldAcessToken, Xamarin.Facebook.AccessToken currentAcessToken)
		{
            if (currentAcessToken != null)
            {
                Console.WriteLine("Facebook access token:" + currentAcessToken.Token);
            }

			var handler = AccessTokenChanged;

			if (handler != null)
			{
				handler(this, currentAcessToken);
			}
		}
	}
}