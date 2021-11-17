
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
        private const string ColletingRequest = "collecting-request";
        private const string SystemConfig = "sys-conf";
        private const string CollectorCancelReason = "cancel-reason";
        private const string Complaint = "complaint";
        private const string SellCollectTransaction = "transaction/sell-collect";
        private const string CollectDealTransaction = "transaction/collect-deal";
        private const string Register = "account-register";

        public static class RegisterApiUrl
        {
            public const string CollectorOtp = Register + "/collector-otp";
            public const string DealerOtp = Register + "/dealer-otp";
        }

        public static class ComplaintApiUrl
        {
            public const string GetSellerComplaint = Complaint + "/seller/get";
            public const string GetCollectorComplaint = Complaint + "/collector/get";
            public const string GetDealerComplaint = Complaint + "/dealer/get";
            public const string ReplySellerComplaint = Complaint + "/seller/reply";
            public const string ReplyCollectorComplaint = Complaint + "/collector/reply";
            public const string ReplyDealerComplaint = Complaint + "/dealer/reply";
        }

        public static class CollectorCancelReasonApiUrl
        {
            public const string Create = CollectorCancelReason + "/create";
            public const string Update = CollectorCancelReason + "/update";
            public const string Delete = CollectorCancelReason + "/delete";
            public const string Get = CollectorCancelReason + "/get";
        }

        public static class SellCollectTransactionApiUrl
        {
            public const string Search = SellCollectTransaction + "/search";
            public const string GetDetail = SellCollectTransaction + "/detail";
        }

        public static class CollectDealTransactionApiUrl
        {
            public const string Search = CollectDealTransaction + "/search";
            public const string GetDetail = CollectDealTransaction + "/detail";
        }

        public static class TransactionAwardAmountApiUrl
        {
            public const string Create = TransactionAwardAmount + "/create";
        }

        public static class TransactionServiceFeeApiUrl
        {
            public const string Create = TransactionServiceFee + "/create";
        }

        public static class CollectingRequestApiUrl
        {
            public const string Search = ColletingRequest + "/search";
            public const string Detail = ColletingRequest + "/detail";
        }

        public static class SystemConfigApiUrl
        {
            public const string SystemConfigInfo = SystemConfig + "/info";
            public const string ModifySystemConfig = SystemConfig + "/modify";
            public const string ModifyTransactionAward = SystemConfig + "/trans-award/modify";
            public const string GetSellCollectTransactionAward = SystemConfig + "/trans-award/sell-collect";
            public const string GetCollectDealTransactionAward = SystemConfig + "/trans-award/collect-deal";
            public const string ModifyTransactionFee = SystemConfig + "/trans-fee/modify";
            public const string GetSellCollectTransactionFee = SystemConfig + "/trans-fee/sell-collect";
            public const string GetCollectDealTransactionFee = SystemConfig + "/trans-fee/collect-deal";
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
