using Amazon;
using System;
using Amazon.Util;
using System.Text;
using Android.App;
using Android.Net;
using Amazon.Lambda;
using Android.Content;
using Newtonsoft.Json;
using Amazon.CognitoSync;
using Amazon.Lambda.Model;
using Announcement.Android;
using Amazon.CognitoIdentity;
using System.Threading.Tasks;
using Amazon.CognitoSync.SyncManager;
using System.Threading;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;

namespace Announcement.Core
{
    public static class AmazonModule
    {
        private static CognitoAWSCredentials Credentials
        {
            get
            {
                if (credentials == null)
                {
                    credentials = new CognitoAWSCredentials (IDENTITY_POOL_ID, regionEndpoint);
                }

                return credentials;
            }
        }
            
        private static CognitoSyncManager SyncManagerInstance
        {
            get
            {
                if (syncManager == null)
                {
                    syncManager = new CognitoSyncManager (Credentials, new AmazonCognitoSyncConfig { RegionEndpoint = regionEndpoint });
                }

                return syncManager;
            }
        }

        private static AmazonLambdaClient LambdaClient
        {
            get
            {
                if (lambdaClient == null)
                {
                    lambdaClient = new AmazonLambdaClient(Credentials, regionEndpoint);

                    lambdaClient.Config.Timeout = TimeSpan.FromSeconds(20);
                }

                return lambdaClient;
            }
        }

        private static AmazonS3Client AmazonS3Client
        {
            get
            {
                if (amazonS3Client == null)
                {
                    var config = new AmazonS3Config();

                    config.ServiceURL = "s3.amazonaws.com";

                    config.UseHttp = true;

                    config.RegionEndpoint = regionEndpoint;

                    amazonS3Client = new AmazonS3Client(Credentials, config);
                }

                return amazonS3Client;
            }
        }

        public static Result<T> InvokeLambda<T>(string methodName, object data)
        {
			if (NetworkAvailability())
			{
				try
				{
					InvokeRequest request = new InvokeRequest();

					request.FunctionName = methodName;

					request.InvocationType = InvocationType.RequestResponse;

					if (data != null)
					{
						request.Payload = JsonConvert.SerializeObject(data);
					}

					var task = LambdaClient.InvokeAsync(request);
				
                    var result = task.Wait(CONNECTION_TIMEOUT_MS);

                    if(result)
                    {
                        var deserializeObject = JsonConvert.DeserializeObject<Result<T>>(Encoding.UTF8.GetString(task.Result.Payload.ToArray()));

                        if(deserializeObject.HasError && deserializeObject.ErrorCode == ErrorCodes.UserWrongAccessToken)
                        {
                            deserializeObject.Message = LocalizationModule.Translate("alert_message_wrong_token");
                        }

                        return deserializeObject;
                    }
                    else
                    {
                        return new Result<T>() { HasError = true, Message = LocalizationModule.Translate("alert_timeout") };
                    }
				}
                catch(TaskCanceledException)
                {
                    return new Result<T>() { HasError = true, Message = LocalizationModule.Translate("alert_timeout") };
                }
				catch (Exception ex)
				{
					return new Result<T>() { HasError = true, Message = ex.Message };
				}
			}
			else
			{
				return new Result<T>() { HasError = true, Message = LocalizationModule.Translate("alert_no_internet_connection") };
			}
        }

        public static async Task<bool> UploadAudioFile(string filePath, string fileName)
        {
            try
            {
                var request = new PutObjectRequest();

                request.BucketName = BUCKET_FOR_AUDIO_FILES;

                request.Key = fileName;

                request.FilePath = filePath;

                var response = await AmazonS3Client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                
            }

            return false;
        }

		private static bool NetworkAvailability()
		{
			var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);

			var info = connectivityManager.ActiveNetworkInfo;

			return info != null && info.IsConnected;
		}
            
        private static CognitoAWSCredentials credentials;

        private static CognitoSyncManager syncManager;

        private static AmazonLambdaClient lambdaClient;

        private static AmazonS3Client amazonS3Client;

        private static RegionEndpoint regionEndpoint = RegionEndpoint.EUWest1;

        private const int CONNECTION_TIMEOUT_MS = 20000;

        private const string BUCKET_FOR_AUDIO_FILES = "stop-spam/audio_records";

        private const string IDENTITY_POOL_ID = "eu-west-1:7e2559ad-767f-41bf-9372-bcfd7b9cd8c6";
    }
}

