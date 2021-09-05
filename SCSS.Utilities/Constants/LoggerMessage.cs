
namespace SCSS.Utilities.Constants
{
    public class AWSLoggerMessage
    {
        public static string UploadFileSuccess(string fileName, FileS3Path path) => $"File {fileName} upload to {path} successfully";

        public static string UploadFileFail(string fileName, FileS3Path path) => $"File {fileName} upload to {path} error";

        public static string GetFileSuccess(string fileName, FileS3Path path) => $"Get File {fileName} in {path} successfully";

        public static string GetFileSuccess(string filepath) => $"Get File with {filepath} error";

        public static string GetFileFail(string fileName, FileS3Path path) => $"Get File {fileName} in {path} error";

        public static string GetFileFail(string filepath) => $"Get File with {filepath} error";
    }


    public class AccountRegistrationLoggerMessage
    {
        public static string RegistrationSuccess(string phone, string role) => $"{role} Account with {phone} sign up successfully";
    }

    public class CacheLoggerMessage
    {
        public static string SetCacheFail(CacheRedisKey key) => $"Set data with key '{key}' into redis error";

        public static string SetCacheSuccess(CacheRedisKey key) => $"Set data with key '{key}' into redis successfully";

        public static string GetCacheFail(CacheRedisKey key) => $"Get data from key '{key}' in redis error";

        public static string RemoveCacheFail(CacheRedisKey key) => $"Remove cache with key '{key}' in redis error";
    }

    public class TwilioLoggerMessage
    {
        public static string SendSMSSuccess(string content, string phoneTo) => $"SMS '{content}' sended to {phoneTo} successfully";

        public static string SendSMSFail(string phoneTo) => $"Send SMS to {phoneTo} error";

    }

    public class FirebaseLoggerMessage
    {

    }
}
