using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Configurations
{
    public class AppSettingKeys
    {
        public static class ConnectionString
        {
            public const string SQLConnectionString = "ConnectionStrings:SQLConnectionString";
            public const string RedisConnectionString = "ConnectionStrings:RedisConnectionString";
        }

        public static class SystemConfig
        {
            public const string CommandTimeOut = "SystemConfig:CommandTimeout";
            public const string ReadScaleOut = "SystemConfig:ReadScaleOut";
            public const string UseSwaggerUI = "SystemConfig:UseSwaggerUI";
            public const string RedisInstanceName = "SystemConfig:RedisInstanceName";
        }

        public static class IdentityServer
        {
            public const string Authority = "IdentityServer:Authority";
            public const string ApiName = "IdentityServer:ApiName";
            public const string ApiSecret = "IdentityServer:ApiSecret";
            public const string ChangeStatus = "IdentityServer:ChangStatus";
            public const string Update = "IdentityServer:Update";
            public const string RegisterDealer = "IdentityServer:RegisterDealer";
            public const string RegisterDealerMember = "IdentityServer:RegisterDealerMember";
            public const string RegisterCollector = "IdentityServer:RegisterCollector";
            public const string RegisterSeller = "IdentityServer:RegisterSeller";
        }

        public static class AWSService
        {
            public const string S3BucketName = "AWSService:S3BucketName";
            public const string S3AccessKey = "AWSService:S3AccessKey";
            public const string S3SecretKey = "AWSService:S3SecretKey";

            public const string CloudWatchAccessKey = "AWSService:CloudWatchAccessKey";
            public const string CloudWatchSecrectKey = "AWSService:CloudWatchSecrectKey";
            public const string CloudWatchLogGroup = "AWSService:CloudWatchLogGroup";

            public const string Region = "AWSService:Region";

        }

        public static class Firebase
        {
            public const string CredentialFile = "FirebaseService:CredentialFile";
        }

        public static class Twilio
        {
            public const string AccountSID = "TwilioService:AccountSID";
            public const string AuthToken = "TwilioService:AuthToken";
            public const string PhoneNumber = "TwilioService:PhoneNumber";
        }

        public static class GoongMap
        {
            public const string GoongApiKey = "GoongMapService:ApiKey";
            public const string GoongApiURL = "GoongMapService:ApiURL";
            public const string GoongDistanceMatrix = "GoongMapService:DistanceMatrix";
            public const string GoongDirection = "GoongMapService:Direction";
        }

        public static class Logging
        {
            public const string Config = "Logging:Config";
        }
    }
}
