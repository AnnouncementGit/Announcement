using Amazon;
using System;
using Amazon.Util;
using Amazon.Lambda;
using Amazon.CognitoSync;
using Amazon.Lambda.Model;
using Amazon.CognitoIdentity;
using Amazon.CognitoSync.SyncManager;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;


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
                }

                return lambdaClient;
            }
        }

        public static Result<T> InvokeLambda<T>(string methodName, object data)
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

                task.Wait();

                return JsonConvert.DeserializeObject<Result<T>>(Encoding.UTF8.GetString(task.Result.Payload.ToArray()));
            }
            catch (Exception ex)
            {
                return new Result<T>() { HasError = true, Message = ex.Message };
            }
        }
            
        private static CognitoAWSCredentials credentials;

        private static CognitoSyncManager syncManager;

        private static AmazonLambdaClient lambdaClient;

        private static RegionEndpoint regionEndpoint = RegionEndpoint.EUWest1;

        private const string IDENTITY_POOL_ID = "eu-west-1:7e2559ad-767f-41bf-9372-bcfd7b9cd8c6";
    }
}

