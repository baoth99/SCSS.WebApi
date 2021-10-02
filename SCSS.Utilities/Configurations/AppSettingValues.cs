
namespace SCSS.Utilities.Configurations
{
    public static class AppSettingValues
    {
        public static int CommandTimeout => ConfigurationHelper.GetValue<int>(AppSettingKeys.SystemConfig.CommandTimeOut);

        public static string SqlConnectionString => ConfigurationHelper.GetValue<string>(AppSettingKeys.ConnectionString.SQLConnectionString);

        public static bool UseSwaggerUI => ConfigurationHelper.GetValue<bool>(AppSettingKeys.SystemConfig.UseSwaggerUI);

        public static bool ReadScaleOut => ConfigurationHelper.GetValue<bool>(AppSettingKeys.SystemConfig.ReadScaleOut);

        public static string RedisConnectionString => ConfigurationHelper.GetValue<string>(AppSettingKeys.ConnectionString.RedisConnectionString);

        public static string RedisInstanceName => ConfigurationHelper.GetValue<string>(AppSettingKeys.SystemConfig.RedisInstanceName);

        public static int RedisDB00 => ConfigurationHelper.GetValue<int>(AppSettingKeys.SystemConfig.RedisDB00);

        public static int RedisDB01 => ConfigurationHelper.GetValue<int>(AppSettingKeys.SystemConfig.RedisDB01);

        public static int RedisDB02 => ConfigurationHelper.GetValue<int>(AppSettingKeys.SystemConfig.RedisDB02);

        public static int RedisDB03 => ConfigurationHelper.GetValue<int>(AppSettingKeys.SystemConfig.RedisDB03);

        public static int RedisDB04 => ConfigurationHelper.GetValue<int>(AppSettingKeys.SystemConfig.RedisDB04);

        public static string Authority => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.Authority);

        public static string ApiName => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.ApiName);

        public static string ApiSecret => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.ApiSecret);

        public static string ID4ChangeStatusUrl => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.ChangeStatus);

        public static string ID4UpdateUrl => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.Update);

        public static string ID4RegisterDealerUrl => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.RegisterDealer);

        public static string ID4RegisterDealerMemberUrl => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.RegisterDealerMember);

        public static string ID4RegisterCollectorUrl => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.RegisterCollector);

        public static string ID4RegisterSellerUrl => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.RegisterSeller);

        public static string AWSS3AccessKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.S3AccessKey);

        public static string AWSS3SecretKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.S3SecretKey);

        public static string AWSS3BucketName => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.S3BucketName);

        public static string AWSRegion => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.Region);

        public static string AWSAccessKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.AWSAccessKey);

        public static string AWSSecrectKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.AWSSecrectKey);

        public static int DurationTimeOutPreSignedUrl => ConfigurationHelper.GetValue<int>(AppSettingKeys.AWSService.DurationTimeOutPreSignedUrl);

        public static string AWSCloudWatchLogGroup => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.CloudWatchLogGroup);

        public static bool IsEnabledSQS => ConfigurationHelper.GetValue<bool>(AppSettingKeys.AWSSQSSetting.Enabled);

        public static bool IsEnabledSQSPublisher => ConfigurationHelper.GetValue<bool>(AppSettingKeys.AWSSQSSetting.EnablePublisher);

        public static bool IsEnabledSubscriber => ConfigurationHelper.GetValue<bool>(AppSettingKeys.AWSSQSSetting.EnableSubscriber);

        public static string NotificationQueueUrl => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSSQSSetting.NotificationQueueUrl);

        public static int MaxNumberOfMessages => ConfigurationHelper.GetValue<int>(AppSettingKeys.AWSSQSSetting.MaxNumberOfMessages);

        public static int WaitTimeSeconds => ConfigurationHelper.GetValue<int>(AppSettingKeys.AWSSQSSetting.WaitTimeSeconds);

        public static string TwilioAccountSID => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.AccountSID);

        public static string TwilioAuthToken => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.AuthToken);

        public static string TwilioPhoneNumber => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.PhoneNumber);

        public static string FirebaseCredentialFile => ConfigurationHelper.GetValue<string>(AppSettingKeys.Firebase.CredentialFile);

        public static string GoogleCredentials => ConfigurationHelper.GetValue<string>(AppSettingKeys.Firebase.GoogleCredentials);

        public static string GoongMapApiKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.GoongMap.GoongApiKey);

        public static string GoongMapApiURL => ConfigurationHelper.GetValue<string>(AppSettingKeys.GoongMap.GoongApiURL);

        public static string GoongDistanceMatrixEndpoint => ConfigurationHelper.GetValue<string>(AppSettingKeys.GoongMap.GoongDistanceMatrix);

        public static string GoongDirectionEndpoint => ConfigurationHelper.GetValue<string>(AppSettingKeys.GoongMap.GoongDirection);

        public static string LoggingConfig => ConfigurationHelper.GetValue<string>(AppSettingKeys.Logging.Config);

        public static string CollectingRequestSQLCommand => ConfigurationHelper.GetValue<string>(AppSettingKeys.SQLCommand.CollectorRequestCommand);

        public static int ResizeImageWidth => ConfigurationHelper.GetValue<int>(AppSettingKeys.ResizeImage.Width);

        public static int ResizeImageHeight => ConfigurationHelper.GetValue<int>(AppSettingKeys.ResizeImage.Height);

        public static int ResizeLimitedHeight => ConfigurationHelper.GetValue<int>(AppSettingKeys.ResizeImage.LimitedHeight);

        public static int ResizeLimitedWidth => ConfigurationHelper.GetValue<int>(AppSettingKeys.ResizeImage.LimitedWidth);

        public static int RatioResize => ConfigurationHelper.GetValue<int>(AppSettingKeys.ResizeImage.Ratio);

        public static string CollectingRequestTrailPeriodSchedule => ConfigurationHelper.GetValue<string>(AppSettingKeys.TaskSchedule.CollectingRequestTrailPeriod);

        public static string CollectingRequestTrailStartSchedule => ConfigurationHelper.GetValue<string>(AppSettingKeys.TaskSchedule.CollectingRequestTrailStart);

        public static int DelayMinutesSchedule => ConfigurationHelper.GetValue<int>(AppSettingKeys.TaskSchedule.DelayMinutes);

    }
}
