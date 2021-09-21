namespace SCSS.WebApi.SystemConstants
{
    public class ScrapSellerApiUrlDefinition
    {
        private const string Account = "seller/account";
        private const string ImageSlider = "image-slider";
        private const string CollectingRequest = "collecting-request";

        public static class DataApiUrl
        {
            public const string GetImage = "image/get";
        }

        public static class AccountApiUrl
        {
            public const string RegisterSellerAccount = Account + "/register";
            public const string UpdateSellerAccount = Account + "/update";
            public const string InfoDetail = Account + "/dealer-info";
            public const string UpdateDeviceId = Account + "/device-id";
            public const string UploadImage = Account + "/upload-image";
        }

        public static class ImageSliderApiUrl
        {
            public const string GetImageSlider = ImageSlider + "/images";
        }

        public static class CollectingRequestApiUrl
        {
            public const string RequestCollecting = CollectingRequest + "/request";
            public const string CancelCollectingRequest = CollectingRequest + "/cancel";
            public const string UploadCollectingRequestImg = CollectingRequest + "/upload-img";
        }

    }

}
