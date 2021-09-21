
namespace SCSS.WebApi.SystemConstants
{
    public class AdminApiUrlDefinition
    {
        private const string Hub = "/hubs/admin";
        private const string Account = "account/admin";
        private const string Data = "data";
        private const string ScrapCategory = "scrap-category";
        private const string ImageSlider = "image-slider";
        private const string RequestRegister = "request-register";
        private const string DealerInformation = "dealer-information";
        private const string TransactionAwardAmount = "transaction-award";
        private const string TransactionServiceFee = "transaction-fee";
        public static class TransactionAwardAmountApiUrl
        {
            public const string Create = TransactionAwardAmount + "/create";
        }

        public static class TransactionServiceFeeApiUrl
        {
            public const string Create = TransactionServiceFee + "/create";
        }

        public static class ScrapCategoryApiUrl
        {
            public const string Search = ScrapCategory + "/search";
            public const string Detail = ScrapCategory + "/detail";
            public const string Image = ScrapCategory + "/image";
        }

        public static class HubApiUrl
        {
            public const string AmountNewAccount = Hub + "/amount/new-user";
            public const string AmountCollectingRequest = Hub + "/amount/collecting-request";
            public const string AmountTransaction = Hub + "/amount/transaction";
        }

        public static class DashboardApiUrl
        {
            public const string Dashboard = "dashboard";
        }

        public static class ImageSliderApiUrl
        {
            public const string Create = ImageSlider + "/create";
            public const string GetList = ImageSlider + "/list";
            public const string GetImages = ImageSlider + "/images";
            public const string GetImageDetail = ImageSlider + "/detail";
            public const string ChangeItem = ImageSlider + "/change";

        }
        public static class AccountApiUrl
        {
            public const string ChangeStatus = Account + "/change-status";
            public const string Search = Account + "/search";
            public const string Detail = Account + "/detail";
        }

        public static class RequestRegisterApiUrl
        {
            public const string SearchDealers = RequestRegister + "/dealers";
            public const string SearchCollectors = RequestRegister + "/collectors";
            public const string DealerDetail = RequestRegister + "/dealer-detail";
            public const string CollectorDetail = RequestRegister + "/collector-detail";
        }

        public static class AdminDataApiUrl
        {
            public const string Role = Data + "/roles";
            public const string Image = Data + "/image";
        }

        public static class DealerInformationUrl
        {
            public const string Search = DealerInformation + "/search";
            public const string GetDetail = DealerInformation + "/detail";
        }
    }
}
