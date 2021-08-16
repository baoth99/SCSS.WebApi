
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

        public static string Authority => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.Authority);

        public static string ApiName => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.ApiName);

        public static string ApiSecret => ConfigurationHelper.GetValue<string>(AppSettingKeys.IdentityServer.ApiSecret);

        public static string AWSS3AccessKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.S3AccessKey);

        public static string AWSS3SecretKey => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.S3SecretKey);

        public static string AWSS3BucketName => ConfigurationHelper.GetValue<string>(AppSettingKeys.AWSService.S3BucketName);

        public static string TwilioAccountSID => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.AccountSID);

        public static string TwilioAuthToken => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.AuthToken);

        public static string TwilioPhoneNumber => ConfigurationHelper.GetValue<string>(AppSettingKeys.Twilio.PhoneNumber);

        public static string FirebaseCredentialFile => ConfigurationHelper.GetValue<string>(AppSettingKeys.Firebase.CredentialFile);
    }
}
