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
            public const string RedisDB00 = "SystemConfig:RedisDB00";
            public const string RedisDB01 = "SystemConfig:RedisDB01";
            public const string RedisDB02 = "SystemConfig:RedisDB02";
            public const string RedisDB03 = "SystemConfig:RedisDB03";
            public const string RedisDB04 = "SystemConfig:RedisDB04";
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
            public const string DurationTimeOutPreSignedUrl = "AWSService:DurationTimeOutPreSignedUrl";

            public const string AWSAccessKey = "AWSService:CloudWatchAccessKey";
            public const string AWSSecrectKey = "AWSService:CloudWatchSecrectKey";
            public const string CloudWatchLogGroup = "AWSService:CloudWatchLogGroup";

            public const string Region = "AWSService:Region";

        }

        public static class AWSSQSSetting
        {
            public const string Enabled = "AWSSQSSetting:Enabled";
            public const string EnablePublisher = "AWSSQSSetting:EnablePublisher";
            public const string EnableSubscriber = "AWSSQSSetting:EnableSubscriber";
            public const string NotificationQueueUrl = "AWSSQSSetting:NotificationQueueUrl";
            public const string MaxNumberOfMessages = "AWSSQSSetting:MaxNumberOfMessages";
            public const string WaitTimeSeconds = "AWSSQSSetting:WaitTimeSeconds";
        }

        public static class Firebase
        {
            public const string CredentialFile = "FirebaseService:CredentialFile";
            public const string GoogleCredentials = "FirebaseService:GoogleCredentials";
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
            public const string GoongAutoComplete = "GoongMapService:AutoComplete";
            public const string GoongPlaceDetail = "GoongMapService:Detail";
            public const string GoongGeocode = "GoongMapService:Geocode";
        }

        public static class Logging
        {
            public const string Config = "Logging:Config";
        }

        public static class SQLCommand
        {
            public const string CollectorRequestCommand = "SqlCommands:CollectorRequestCommands";
            public const string StatisticCommands = "SqlCommands:StatisticCommands";
            public const string FeedbackCommands = "SqlCommands:FeedbackCommands";
        }

        public static class ResizeImage
        {
            public const string LimitedWidth = "ResizeImage:LimitedWidth";
            public const string LimitedHeight = "ResizeImage:LimitedHeight";
            public const string Ratio = "ResizeImage:Ratio";
            public const string Width = "ResizeImage:Width";
            public const string Height = "ResizeImage:Height";
        }

        public static class TaskSchedule
        {
            public const string CollectingRequestTrailPeriod = "TaskSchedule:CollectingRequestTrailPeriod";
            public const string CollectingRequestTrailStart = "TaskSchedule:CollectingRequestTrailStart";
            public const string DelayMinutes = "TaskSchedule:DelayMinutes";
            public const string NoticeToCollector = "TaskSchedule:NoticeToCollector";
        }
    }
}
