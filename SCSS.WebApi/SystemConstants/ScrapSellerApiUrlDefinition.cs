namespace SCSS.WebApi.SystemConstants
{
    public class ScrapSellerApiUrlDefinition
    {
        private const string Account = "seller/account";
        private const string ImageSlider = "image-slider";
        private const string CollectingRequest = "collecting-request";
        private const string Notification = "notification";
        private const string Activity = "activity";
        private const string Feedback = "feedback";
        private const string Map = "map";
        public static class DataApiUrl
        {
            public const string GetImage = "image/get";
        }

        public static class NotificationApiUrl
        {
            public const string Get = Notification + "/get";
            public const string GetDetail = Notification + "/get-detail";
            public const string GetNumberOfUnReadNotifications = Notification + "/unread-count";
            public const string Read = Notification + "/read";
        }

        public static class AccountApiUrl
        {
            public const string RegisterSellerAccount = Account + "/register";
            public const string UpdateSellerAccount = Account + "/update";
            public const string InfoDetail = Account + "/seller-info";
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
            public const string GetOperatingTimeRange = CollectingRequest + "/operating-time";
            public const string CancelCollectingRequest = CollectingRequest + "/cancel";
            public const string UploadCollectingRequestImg = CollectingRequest + "/upload-img";
            public const string GetRemainingDays = CollectingRequest + "/remaining-days";
            public const string RequestAbility = CollectingRequest + "/request-ability";
        }

        public static class ActivityApiUrl
        {
            public const string Get = Activity + "/get";
            public const string Detail = Activity + "/detail";

        }

        public static class FeedbackApiUrl
        {
            public const string CreateTransFeedback = Feedback + "/trans-feedback/create";
            public const string CreateFeedbackToAdmin = Feedback + "/feedback-admin/create";
        }

        public static class MapApiUrl
        {
            public const string AutoComplete = Map + "/place/autoComplete";
            public const string PlaceDetail = Map + "/place/detail";
            public const string ReverseGeocoding = Map + "/geocode";
        }
    }

}
