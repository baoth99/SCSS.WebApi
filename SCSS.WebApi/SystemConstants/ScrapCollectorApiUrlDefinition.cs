
namespace SCSS.WebApi.SystemConstants
{
    public class ScrapCollectorApiUrlDefinition
    {
        private const string ScrapCategory = "scrap-category";
        private const string Account = "collector/account";
        private const string CollectingRequest = "collecting-request";
        private const string Hub = "/hubs/collector";
        public static class AccountApiUrl
        {
            public const string RegisterCollectorAccount = Account + "/register";
            public const string UpdateCollectorAccount = Account + "/update";
            public const string UpdateDeviceId = Account + "/device-id";
            public const string UploadImage = Account + "/upload-image";

        }

        public static class HubApiUrl
        {
            public const string CollectingRequest = Hub;
            
        }

        public static class CollectingRequestApiUrl
        {
            public const string Get = CollectingRequest + "/get";
            public const string Detail = CollectingRequest + "/detail";
            public const string Receive = CollectingRequest + "/receive";
            public const string Reject = CollectingRequest + "/reject";
            public const string GetReceivedList = CollectingRequest + "/receive/get";
            public const string GetReceivedDetail = CollectingRequest + "/receive/detail";
            public const string Cancel = CollectingRequest + "/cancel";
        }

        public static class DataApiUrl
        {
            public const string GetImage = "image/get";
        }

        public static class ScrapCategoryUrl
        {
            public const string Create = ScrapCategory + "/create";
            public const string Get = ScrapCategory + "/get";
            public const string GetDetail = ScrapCategory + "/get-detail";
            public const string CheckName = ScrapCategory + "/check-name";
            public const string Update = ScrapCategory + "/update";
            public const string UploadImage = ScrapCategory + "/upload-image";
            public const string Remove = ScrapCategory + "/remove";
        }
    }
}
