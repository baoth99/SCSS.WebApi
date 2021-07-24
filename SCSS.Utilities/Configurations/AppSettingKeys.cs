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
        }

        public static class AWSService
        {
            public const string S3BucketName = "AWSService:S3BucketName";
            public const string S3AccessKey = "AWSService:S3AccessKey";
            public const string S3SecretKey = "AWSService:S3SecretKey";
        }

        public static class ESMS
        {
            public const string ApiKey = "ESMSService:ApiKey";
            public const string SecrectKey = "ESMSService:SecretKey";
            public const string BrandName = "ESMSService:BrandName";
            public const string ApiUrl = "ESMSService:ApiUrl";
        }
    }
}
